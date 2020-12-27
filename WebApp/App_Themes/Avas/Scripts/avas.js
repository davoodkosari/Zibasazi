
$(document).ready(function () {


    var containerEl = document.querySelector('.mixed');
    var mixer = mixitup(containerEl);



    $(".avas .section-person-info .owl-carousel").owlCarousel({
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
    $('.avas .section-supporters-box .owl-carousel').owlCarousel({
        dots: false,
        loop: true,
        margin: 20,
        rtl: true, autoplay: true,
        autoplayTimeout: 3000, navText: ['<i class="fas fa-long-arrow-alt-right"></i>', '<i class="fas fa-long-arrow-alt-left"></i>'],
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
    $('.avas .gallerypage-box .owl-carousel').owlCarousel({
        dots: false,
        loop: true,
        margin: 0,
        rtl: true, autoplay: true,
        autoplayTimeout: 3000, navText: ['<i class="fas fa-long-arrow-alt-right"></i>', '<i class="fas fa-long-arrow-alt-left"></i>'],
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



});




