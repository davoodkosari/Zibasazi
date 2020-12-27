using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using Microsoft.Win32;
using Radyn.XmlModel;

namespace Radyn.AttendanceWinApp
{

    public partial class MainWindow : Window
    {


        public MainWindow()
        {

            InitializeComponent();


        }

        private AttendanceXml _attendanceXml;

        public AttendanceXml AttendanceXml
        {
            get
            {
                if (_attendanceXml == null)
                    _attendanceXml = new AttendanceXml();
                return _attendanceXml;
            }
        }

        private CongressXml _homaXml;

        public CongressXml HomaXml
        {
            get
            {
                if (_homaXml == null)
                    _homaXml = new CongressXml();
                return _homaXml;
            }
            set { _homaXml = value; }
        }


        private XmlDocument _xmlDocument;

        public XmlDocument XmlDocument
        {
            get
            {
                if (_xmlDocument == null)
                    _xmlDocument = new XmlDocument();
                return _xmlDocument;
            }

        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

            txtcode.Focus();
            HomaXml = GetHomaXml();
            if (_homaXml == null)
            {
                groupitemlist.Visibility = Visibility.Collapsed;
                return;
            }
            groupitemlist.Visibility = HomaXml.AttendanceType.Any() ? Visibility.Visible : Visibility.Collapsed;
            drpCongressrefId.ItemsSource = HomaXml.GetCongressList();
            drpCongressrefId.SelectedIndex = 0;

            drpType.ItemsSource = HomaXml.AttendanceType;
            drpType.SelectedIndex = 0;


            DrpType_OnSelectionChanged(null, null);

        }



        private void Btnsave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {

                var list = new List<string>();
                if (string.IsNullOrEmpty(txtcode.Text)) list.Add("لطفا کد را وارد نماید");
                if (drpType.SelectedValue == null) list.Add("لطفا نوع را انتخاب نماید");
                if (drpCongressrefId.SelectedValue == null) list.Add("لطفا همایش را انتخاب نمایید");

                var aggregate = list.Aggregate("", (current, str) => current + Environment.NewLine + (str));
                if (!string.IsNullOrEmpty(aggregate))
                {
                    MessageBox.Show(aggregate, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var absentItem = new AbsentItemXml() { Value = txtcode.Text, Date = DateTime.Now.ToString() };
                var congressId = Guid.Parse(drpCongressrefId.SelectedValue.ToString());
                var congressModelXml = HomaXml.CongressModelXml.FirstOrDefault(x => x.CongressId == congressId);

                if (congressModelXml == null) return;
                switch (drpType.SelectedValue)
                {
                    case "WorkShop":
                        if (drpWorkShoprefId.SelectedValue == null)
                        {
                            MessageBox.Show("لطفا کارگاه را انتخاب نمایید");
                            return;
                        }
                        var workshopId = Guid.Parse(drpWorkShoprefId.SelectedValue.ToString());

                        var @default = congressModelXml.WorkShopModelList.FirstOrDefault(x => x.Key == workshopId.ToString());
                        if (@default == null) return;
                        var orDefault = @default.UserList.FirstOrDefault(x => x.Key == absentItem.Value);
                        if (orDefault == null)
                            throw new Exception(string.Format("کاربری با شماره {0} در {1} ثبت نشده است", absentItem.Value, drpWorkShoprefId.Text));

                        var firstOrDefault = AttendanceXml.WorkShopAbsentList.FirstOrDefault(x => x.CongressId == congressId && x.WorkShopId == workshopId);
                        if (firstOrDefault == null)
                        {
                            firstOrDefault = new WorkShopAbsentXml() { CongressId = congressId, WorkShopId = workshopId };
                            firstOrDefault.AbsentItem.Add(absentItem);
                            AttendanceXml.WorkShopAbsentList.Add(firstOrDefault);
                        }
                        else
                            firstOrDefault.AbsentItem.Add(absentItem);
                        lstuseritems.SelectedValue = orDefault.Value;
                        lstuseritems.ScrollIntoView(orDefault.Value);
                        lstitems.Items.Add(string.Format("کاربری {0} تاریخ {1} در {2}", orDefault.Value, absentItem.Date, drpWorkShoprefId.Text));
                        break;
                    case "Congress":

                        var keyValueXml = congressModelXml.UserList.FirstOrDefault(x => x.Key == absentItem.Value);
                        if (keyValueXml == null)
                            throw new Exception(string.Format("کاربری با شماره {0} در {1} ثبت نشده است", absentItem.Value, drpCongressrefId.Text));
                        var absentXml = AttendanceXml.CongressAbsentList.FirstOrDefault(x => x.CongressId == congressId);
                        if (absentXml == null)
                        {

                            absentXml = new CongressAbsentXml() { CongressId = congressId };
                            absentXml.AbsentItem.Add(absentItem);
                            AttendanceXml.CongressAbsentList.Add(absentXml);
                        }
                        else
                            absentXml.AbsentItem.Add(absentItem);

                        lstuseritems.SelectedValue = keyValueXml.Value;
                        lstuseritems.ScrollIntoView(keyValueXml.Value);
                        lstitems.Items.Add(string.Format("کاربر {0} تاریخ {1} در {2}", keyValueXml.Value, absentItem.Date, drpCongressrefId.Text));
                        break;
                    case "Food":
                        break;
                }
                XmlDocument.InnerXml = AttendanceXml.XmlSeserialize(); 
                XmlDocument.Save(Constast.UploadxmlAddress);
                txtcode.Clear();
                txtcode.Focus();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }


        private void Btndownload_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txturl.Text))
                {
                    MessageBox.Show("لطفا  مسیر فایل را وارد نمایید", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

              
                var xmlDoc = new XmlDocument();
                string Json;
                var webRequest = (HttpWebRequest)WebRequest.Create(txturl.Text + Constast.DownloadUrl);
                webRequest.Method = "Get";
                webRequest.ContentType = "application/json";
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        var rdr = new StreamReader(responseStream, Encoding.UTF8);
                        Json = rdr.ReadToEnd();

                    }
                }

                var jsonSerializer = Serialize.JsonDeserialize<object>(Json);
                var convertHtmlToString = System.Net.WebUtility.HtmlDecode(jsonSerializer.ToString());
                var validjsonSerializer = Serialize.JsonDeserialize<CongressXml>(convertHtmlToString);
                xmlDoc.InnerXml = validjsonSerializer.XmlSeserialize();
                xmlDoc.Save(Constast.DownloadXmlAddress);
                MainWindow_OnLoaded(null, null);
                MessageBox.Show("اطلاعات با موفقیت دانلود شد", "", MessageBoxButton.OK, MessageBoxImage.Information);
              
            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.Message);
            }



        }



        private void Btnupload_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txturl.Text))
                {
                    MessageBox.Show("لطفا  مسیر فایل را وارد نمایید", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var jsonSerializer = AttendanceXml.JsonSerializer();
                var webRequest = (HttpWebRequest)WebRequest.Create(txturl.Text + Constast.UploadUrl);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonSerializer);
                webRequest.ContentLength = byteArray.Length;
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }
                var resp = (HttpWebResponse)webRequest.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK)
                    MessageBox.Show("اطلاعات با موفقیت ارسال شد", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }



        public static CongressXml GetHomaXml()
        {

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(Constast.DownloadXmlAddress);
                return Serialize.XmlDeserialize<CongressXml>(xmlDoc.InnerXml);
            }
            catch (Exception ex)
            {
                return null;

            }


        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Btnsave_OnClick(null, null);
        }
        private void Btnuploadfile_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog saveFileDialog = new OpenFileDialog();
                saveFileDialog.Filter = "Text documents (.xml)|*.xml";
                var xmlDoc = new XmlDocument();
                if (saveFileDialog.ShowDialog() == true)
                {
                    xmlDoc.Load(saveFileDialog.FileName);
                    xmlDoc.Save(Constast.DownloadXmlAddress);
                    MainWindow_OnLoaded(null, null);
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد", "", MessageBoxButton.OK, MessageBoxImage.Information);

                }



            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }


        private void Btnsavefile_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text documents (.xml)|*.xml";
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, XmlDocument.InnerXml, Encoding.UTF8);
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }


        private void DrpCongressrefId_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lstuseritems.Items.Clear();
                if (drpCongressrefId.SelectedValue == null) return;
                var congressId = Guid.Parse(drpCongressrefId.SelectedValue.ToString());
                drpWorkShoprefId.ItemsSource = HomaXml.GetWorkShopList(congressId);
                drpWorkShoprefId.SelectedIndex = 0;
                ShowList();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void DrpWorkShoprefId_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ShowList();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void DrpType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (drpType.SelectedValue == null)
            {

                return;
            }
            switch (drpType.SelectedValue)
            {
                case "WorkShop":
                    lblworkshop.Visibility = Visibility.Visible;
                    drpWorkShoprefId.Visibility = Visibility.Visible;
                    break;
                case "Congress":
                    lblworkshop.Visibility = Visibility.Collapsed;
                    drpWorkShoprefId.Visibility = Visibility.Collapsed;

                    break;

                case "Food":
                    break;
            }
            ShowList();

        }

        private void Btnclose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }




        public void ShowList()
        {

            try
            {


                lstuseritems.Items.Clear();
                if (drpType.SelectedValue == null) return;
                if (drpCongressrefId.SelectedValue == null) return;
                var congressId = Guid.Parse(drpCongressrefId.SelectedValue.ToString());
                var congressModelXml = HomaXml.CongressModelXml.FirstOrDefault(x => x.CongressId == congressId);
                if (congressModelXml == null) return;
                switch (drpType.SelectedValue)
                {
                    case "WorkShop":
                        if (drpWorkShoprefId.SelectedValue == null)
                        {
                            MessageBox.Show("لطفا کارگاه را انتخاب نمایید");
                            return;
                        }
                        var workshopId = Guid.Parse(drpWorkShoprefId.SelectedValue.ToString());
                        var @default = congressModelXml.WorkShopModelList.FirstOrDefault(x => x.Key == workshopId.ToString());
                        if (@default == null) return;
                        var keyValueXmls = @default.UserList.OrderBy(x => x.Value);
                        foreach (var keyValueXml in keyValueXmls) lstuseritems.Items.Add(keyValueXml.Value);
                        break;
                    case "Congress":
                        var valueXmls = congressModelXml.UserList.OrderBy(x => x.Value);
                        foreach (var keyValueXml in valueXmls) lstuseritems.Items.Add(keyValueXml.Value);
                        break;
                    case "Food":
                        break;
                }




            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }


    }
}
