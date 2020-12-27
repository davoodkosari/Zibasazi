

/****** Object:  UserDefinedFunction [dbo].[UDF_Gregorian_To_Persian]    Script Date: 9/8/2016 9:14:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Function [dbo].[UDF_Gregorian_To_Persian] (@date datetime)
Returns nvarchar(50)
as
Begin
    Declare @depoch as bigint
    Declare @cycle  as bigint
    Declare @cyear  as bigint
    Declare @ycycle as bigint
    Declare @aux1 as bigint
    Declare @aux2 as bigint
    Declare @yday as bigint
    Declare @Jofst  as Numeric(18,2)
    Declare @jdn bigint
 
    Declare @iYear   As Integer
    Declare @iMonth  As Integer
    Declare @iDay    As Integer


	 Declare @tmpiMonth  As nvarchar(50)
    Declare @tmpiDay    As nvarchar(50)

 
    Set @Jofst=2415020.5
    Set @jdn=Round(Cast(@date as int)+ @Jofst,0)
 
    Set @depoch = @jdn - [dbo].[UDF_Persian_To_Julian](475, 1, 1) 
    Set @cycle = Cast(@depoch / 1029983 as int) 
    Set @cyear = @depoch%1029983 
 
    If @cyear = 1029982
       Begin
         Set @ycycle = 2820 
       End
    Else
       Begin
        Set @aux1 = Cast(@cyear / 366 as int) 
        Set @aux2 = @cyear%366 
        Set @ycycle = Cast(((2134 * @aux1) + (2816 * @aux2) + 2815) / 1028522 as int) + @aux1 + 1 
      End
 
    Set @iYear = @ycycle + (2820 * @cycle) + 474 
 
    If @iYear <= 0
      Begin 
        Set @iYear = @iYear - 1 
      End
    Set @yday = (@jdn - [dbo].[UDF_Persian_To_Julian](@iYear, 1, 1)) + 1 
    If @yday <= 186 
       Begin
         Set @iMonth = CEILING(Convert(Numeric(18,4),@yday) / 31) 
       End
    Else
       Begin
          Set @iMonth = CEILING((Convert(Numeric(18,4),@yday) - 6) / 30)  
       End
       Set @iDay = (@jdn - [dbo].[UDF_Persian_To_Julian](@iYear, @iMonth, 1)) + 1 
 




 --Convert Day to 2 char For Example 02 :D // by ali bairampour
		set @tmpiDay = Convert(nvarchar(50),@iDay)
		If @iDay <=9
      Begin 
        Set @tmpiDay ='0'+ Convert(nvarchar(50),@iDay)
      End

	  --Convert Month to 2 char For Example 02 :D // by ali bairampour
	  	set @tmpiMonth = Convert(nvarchar(50),@iMonth)
		If @iMonth <=9
      Begin 
        Set @tmpiMonth ='0'+ Convert(nvarchar(50),@iMonth)
      End
	  

      Return @tmpiDay + '-' +   @tmpiMonth +'-' + Convert(nvarchar(50),@iYear)
End

GO


-------------------------------------------------------------------------------------



/****** Object:  UserDefinedFunction [dbo].[UDF_Julian_To_Gregorian]    Script Date: 9/8/2016 9:14:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-----------------------------------------------------------------------------




/****** Object:  UserDefinedFunction [dbo].[UDF_Persian_To_Julian]    Script Date: 9/8/2016 9:14:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

 --First we need to convert Persian calendar date to Julian Calendar date
Create FUNCTION [dbo].[UDF_Persian_To_Julian](@iYear int,@iMonth int,@iDay int)
RETURNS bigint
AS
Begin

Declare @PERSIAN_EPOCH  as int
Declare @epbase as bigint
Declare @epyear as bigint
Declare @mdays as bigint
Declare @Jofst  as Numeric(18,2)
Declare @jdn bigint

Set @PERSIAN_EPOCH=1948321
Set @Jofst=2415020.5

If @iYear>=0 
    Begin
        Set @epbase=@iyear-474 
    End
Else
    Begin
        Set @epbase = @iYear - 473 
    End
    set @epyear=474 + (@epbase%2820) 
If @iMonth<=7
    Begin
        Set @mdays=(Convert(bigint,(@iMonth) - 1) * 31)
    End
Else
    Begin
        Set @mdays=(Convert(bigint,(@iMonth) - 1) * 30+6)
    End
    Set @jdn =Convert(int,@iday) + @mdays+ Cast(((@epyear * 682) - 110) / 2816 as int)  + (@epyear - 1) * 365 + Cast(@epbase / 2820 as int) * 1029983 + (@PERSIAN_EPOCH - 1) 
    RETURN @jdn
End

GO


