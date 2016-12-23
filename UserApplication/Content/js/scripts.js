var appMaster = {

    preLoader: function () {
        imageSources = []
        $('img').each(function () {
            var sources = $(this).attr('src');
            imageSources.push(sources);
        });
        if ($(imageSources).load()) {
            $('.pre-loader').fadeOut('slow');
        }
    },

    smoothScroll: function () {
        // Smooth Scrolling
        $('a[href*=#]:not([href=#carousel-example-generic])').click(function () {
            if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {

                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                if (target.length) {
                    $('html,body').animate({
                        scrollTop: target.offset().top
                    }, 1000);
                    return false;
                }
            }
        });
    },

    reviewsCarousel: function () {
        // Reviews Carousel
        $('.review-filtering').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            dots: true,
            arrows: false,
            autoplay: true,
            autoplaySpeed: 5000
        });
    },

    screensCarousel: function () {
        // Screens Carousel
        $('.filtering').slick({
            slidesToShow: 4,
            slidesToScroll: 4,
            dots: false,
            responsive: [{
                breakpoint: 1024,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2,
                    infinite: true,
                    dots: true
                }
            }, {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            }, {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }]
        });

        $('.js-filter-all').on('click', function () {
            $('.filtering').slickUnfilter();
            $('.filter a').removeClass('active');
            $(this).addClass('active');
        });

        $('.js-filter-one').on('click', function () {
            $('.filtering').slickFilter('.one');
            $('.filter a').removeClass('active');
            $(this).addClass('active');
        });

        $('.js-filter-two').on('click', function () {
            $('.filtering').slickFilter('.two');
            $('.filter a').removeClass('active');
            $(this).addClass('active');
        });

        $('.js-filter-three').on('click', function () {
            $('.filtering').slickFilter('.three');
            $('.filter a').removeClass('active');
            $(this).addClass('active');
        });

    },

    animateScript: function () {
        $('.scrollpoint.sp-effect1').waypoint(function () { $(this).toggleClass('active'); $(this).toggleClass('animated fadeInLeft'); }, { offset: '100%' });
        $('.scrollpoint.sp-effect2').waypoint(function () { $(this).toggleClass('active'); $(this).toggleClass('animated fadeInRight'); }, { offset: '100%' });
        $('.scrollpoint.sp-effect3').waypoint(function () { $(this).toggleClass('active'); $(this).toggleClass('animated fadeInDown'); }, { offset: '100%' });
        $('.scrollpoint.sp-effect4').waypoint(function () { $(this).toggleClass('active'); $(this).toggleClass('animated fadeIn'); }, { offset: '100%' });
        $('.scrollpoint.sp-effect5').waypoint(function () { $(this).toggleClass('active'); $(this).toggleClass('animated fadeInUp'); }, { offset: '100%' });
    },

    revSlider: function () {

        var docHeight = $(window).height();


        var mainSlider = $('.tp-banner').revolution({
            delay: 9000,
            startwidth: 1170,
            startheight: docHeight,
            hideThumbs: 10,
            touchenabled: false,
            fullWidth: "on",
            hideTimerBar: "on",
            fullScreen: "on",
            onHoverStop: "off",
            fullScreenOffsetContainer: ""
        });

    },

    scrollMenu: function () {
        var num = 50; //number of pixels before modifying styles

        $(window).bind('scroll', function () {
            if ($(window).scrollTop() > num) {
                $('nav').addClass('scrolled');

            } else {
                $('nav').removeClass('scrolled');
            }
        });
    },
    placeHold: function () {
        // run Placeholdem on all elements with placeholders
        Placeholdem(document.querySelectorAll('[placeholder]'));
    }

}; // AppMaster


$(document).ready(function () {

    appMaster.smoothScroll();

    appMaster.reviewsCarousel();

    appMaster.screensCarousel();

    appMaster.animateScript();

    appMaster.revSlider();

    appMaster.scrollMenu();

    appMaster.placeHold();

});

$(".knob").knob();

$(".registerMe").click(function () {
    $("#loginModal").modal("hide");
    $("#MobileModal").modal("hide");
});
$("#androidApp").click(function() {
    $("#MobileModal").modal("show");
});

$("#loginBtn").click(function () {
    $("#loginModal").modal("show");
    return false;
});


$('#registerForm').validate({
    rules: {
        mobile: {
            required: true,
            number: true
        },
        password: {
            required: true
        }
    },
    lang: 'fa',
    highlight: function (element) {
        $(element).closest('.control-group').removeClass('success').addClass('error');
    },
    success: function (element) {
        element.remove();
    }
});

$('#loginForm').validate({
    rules: {
        loginmobile: {
            required: true,
            number: true
        },
        pass: {
            required: true
        }
    },
    lang: 'fa',
    highlight: function (element) {
        $(element).closest('.control-group').removeClass('success').addClass('error');
    },
    success: function (element) {
        element.remove();
    }
});

//form submittion
$('#registerForm').submit(function (event) {
    if ($('#registerForm').valid()) {
        var $this = $(this);
        event.preventDefault();
        $this.find('button[type=submit]').text("لطفا صبر کنید...");
        $('.register-alert-wrap').children().remove();
        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: '/user/RegisterUser',
            data: $this.serializeArray(),
            success: function (datastr) {
                switch (datastr.Status) {
                    case "OK":
                        $("#mobile").attr('disabled', true);
                        $(".register-alert-wrap").append('<div class="alert alert-success">ثبت با موفقیت انجام شد.در حال انتقال به برنامه..</div>');
                        var appLink = $('#appLink').val();
                        window.location.replace(appLink);
                        break;
                    case "Confirmation":
                        var mobile = $("#mobile").val();
                        $("#userMobile").text(mobile);
                        $("#registerForm").modal("hide");
                        $("#confirmModal").modal("show");
                        break;
                    case "BadRequest":
                        $.each(datastr.Errors, function (key, value) {
                            $(".register-alert-wrap").append('<div class="alert alert-danger"><strong>خطا : </strong>' + value.Message + '</div>');
                        });
                        $this.find('button[type=submit]').text("ثبت");
                        break;
                    case "UnAuthorized":
                        var loginhref = $('#loginurl').val;
                        $this.find('button[type=submit]').text("ثبت");
                        break;
                    case "WrongInfo":
                        $this.find('button[type=submit]').text("ثبت");
                        break;
                    default:

                }
            }
        }).error(function (error) {
            $('.register-alert-wrap').append('<div class="alert alert-danger"><strong>خطا : </strong>خطا در ارتباط</div>');
            $this.find('button[type=submit]').text("ثبت");
        });
    };
});

$('#loginForm').submit(function (event) {
    if ($('#loginForm').valid()) {
        var $this = $(this);
        event.preventDefault();
        $this.find('button[type=submit]').text("لطفا صبر کنید...");
        $('.login-alert-wrap').children().remove();
        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: '/user/LoginUser',
            data: $this.serializeArray(),
            success: function (datastr) {
                switch (datastr.Status) {
                    case "OK":
                        var appLink = $('#appLink').val();
                        $(".login-alert-wrap").append('<div class="alert alert-success">در حال انتقال به برنامه...</div>');
                        window.location.replace(appLink);
                        break;
                    case "Confirmation":
                        var mobile = $("#loginmobile").val();
                        $("#userMobile").text(mobile);
                        $("#loginModal").modal("hide");
                        $("#confirmModal").modal("show");
                        break;
                    case "Error":
                        $(".login-alert-wrap").append('<div class="alert alert-danger"><strong>خطا : </strong>' + datastr.error + '</div>');
                        break;
                    case "UnAuthorized":
                        var loginhref = $('#loginurl').val;
                        break;
                    case "WrongInfo":
                        break;
                    default:

                }
                $this.find('button[type=submit]').text("ورود");
            }
        }).error(function (error) {
            $('.login-alert-wrap').append('<div class="alert alert-danger"><strong>خطا : </strong>خطا در ارتباط</div>');
            $this.find('button[type=submit]').text("ورود");
        });
    };
});


$('#contactForm').submit(function (event) {
        var $this = $(this);
        event.preventDefault();
        $this.find('button[type=submit]').text("لطفا صبر کنید...");
        $('.contact-alert-wrap').children().remove();
        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: '/user/SubmitContactUs',
            data: $this.serializeArray(),
            success: function (datastr) {
                switch (datastr.Status) {
                    case "OK":
                        $(".contact-alert-wrap").append('<div class="alert alert-success">از اینکه به بهبود کار ما کمک می کنید سپاسگزاریم.</div>');
                        break;
                    case "Error":
                        $(".contact-alert-wrap").append('<div class="alert alert-danger"><strong>خطا : </strong>' + datastr.error + '</div>');
                        break;
                    default:
                }
                $this.find('button[type=submit]').text("ثبت");
            }
        }).error(function (error) {
            $('.contact-alert-wrap').append('<div class="alert alert-danger"><strong>خطا : </strong>خطا در ارتباط</div>');
            $this.find('button[type=submit]').text("ثبت");
        });
});

$('#confirmModal').submit(function (event) {
    var $this = $(this);
    event.preventDefault();
    $this.find('button[type=submit]').text("لطفا صبر کنید...");
    $('.confirm-alert-wrap').children().remove();
    var mobile=$("#userMobile").text();
    $.ajax({
        dataType: 'json',
        type: 'POST',
        url: '/user/ConfirmMobile',
        data: {mobileNo:mobile},
        success: function (datastr) {
            if (datastr) {
                var appLink = $('#appLink').val();
                $(".confirm-alert-wrap").append('<div class="alert alert-success">در حال انتقال به برنامه...</div>');
                window.location.replace(appLink);
            } else {
                $(".confirm-alert-wrap").append('<div class="alert alert-danger"><strong>خطا : </strong>لطفا پس از چند دقیقه دوباره تایید نمایید</div>');
            }
            $this.find('button[type=submit]').text("تایید");
        }
    }).error(function (error) {
        $('.contact-alert-wrap').append('<div class="alert alert-danger"><strong>خطا : </strong>خطا در ارتباط</div>');
        $this.find('button[type=submit]').text("تایید");
    });
});

