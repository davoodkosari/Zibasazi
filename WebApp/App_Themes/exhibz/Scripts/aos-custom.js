$(document).ready(function () {


    $(".avas .news-box").addClass("aos-item");
    $(".section-supporters-box > div").addClass("aos-item");
    $(".news-section .title-banner").addClass("aos-item");
    $(".news-section .login-main").addClass("aos-item");

    $(".news-section .title-banner").attr("data-aos", "fade-left");
    $(".news-section .login-main").attr("data-aos", "fade-right");
    $(".avas .news-box").attr("data-aos", "fade-up");
    $(".section-supporters-box > div").attr("data-aos", "zoom-in");
    AOS.init({
        once: true,
        easing: "ease-in-out-sine"
    });



});



