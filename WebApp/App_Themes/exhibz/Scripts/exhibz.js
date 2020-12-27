
$(document).ready(function() {


    //    var containerEl = document.querySelector('.mixed');
    //    var mixer = mixitup(containerEl);

    if ($('.main_slider').length > 0) {
        $.setSettingSlider(".slide-show.main_slider",
            {
                height: "700px",
                width: "100%",
                isShow: true,
                slider_control: "icon",
                name: "main_slider",
                type_change: "none",
                time_change: 10000

            });
    }

    $(".exhibz .section-person-info .owl-carousel").owlCarousel({
        dots: false,
        margin: 40,
        loop: true,
        rtl: true,
        autoplay: true,
        autoplayTimeout: 3000,
        navText: ['<i class="fas fa-long-arrow-alt-right"></i>', '<i class="fas fa-long-arrow-alt-left"></i>'],
        autoplayHoverPause: true,
        nav: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3
            }
        }
    });
    $('.exhibz .section-supporters-box .owl-carousel').owlCarousel({
        dots: false,
        loop: true,
        margin: 20,
        rtl: true,
        autoplay: true,
        autoplayTimeout: 3000,
        navText: ['<i class="fas fa-long-arrow-alt-right"></i>', '<i class="fas fa-long-arrow-alt-left"></i>'],
        autoplayHoverPause: true,
        nav: true,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 6
            },
            1000: {
                items: 8
            }
        }
    });
    $('.exhibz .gallerypage-box .owl-carousel').owlCarousel({
        dots: false,
        loop: true,
        margin: 0,
        rtl: true,
        autoplay: true,
        autoplayTimeout: 3000,
        navText: ['<i class="fas fa-long-arrow-alt-right"></i>', '<i class="fas fa-long-arrow-alt-left"></i>'],
        autoplayHoverPause: true,
        nav: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    });


    $(".image-gallery-category")
        .mouseenter(function() {
            $(this).removeClass("mouseleave");
            $(this).addClass("mouseenter");

        })
        .mouseleave(function() {
            $(this).removeClass("mouseenter");
            $(this).addClass("mouseleave");
        });


    $(".image-gallery-category").hover(function() {

        $(".image-gallery-category").removeClass();
        $(this).addClass("hover");

        if ($(this).hasClass("icon-nav-change")) {

            $(this).addClass("hover");
        } else {
            $(this).addClass("hover");
        }

    });
});