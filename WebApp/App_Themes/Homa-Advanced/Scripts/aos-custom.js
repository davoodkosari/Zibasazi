$(document).ready(function () {


    $(".Homa-Advanced .container-box > div").addClass("aos-item");
    $(".Homa-Advanced .widget-content > div").addClass("aos-item");
    $(".Homa-Advanced .widget-content > div > a").addClass("aos-item");
    $(".Homa-Advanced .widget-content > p").addClass("aos-item");

    $(".Homa-Advanced .footer ").addClass("aos-item");
    $(".Homa-Advanced .aos-item").attr("data-aos", "zoom-in");
    $(".Homa-Advanced .aos-item").attr("data-aos-anchor-placement", "top-bottom");
    $(".Homa-Advanced .footer").attr("data-aos", "fade-up");

    AOS.init({
        once: true,
        easing: "ease-in-out-sine"
    });



});



