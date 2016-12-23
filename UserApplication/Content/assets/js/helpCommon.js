var DocMaster = {
    treeMenu: function () {
        $(".sidebar-nav > li > a").on('click', function (e) {
            $(this).siblings("ul").slideToggle();
            e.preventDefault();
        });
    },
    sideToggle: function () {
        $("#menu-toggle").click(function (e) {
            e.preventDefault();
            $("#wrapper").toggleClass("active");
        });
    },
    scrollToElement: function () {
        $('.sidebar-nav li a[href*=#]:not([href=#])').click(function () {
            if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                if (target.length) {
                    $('html, body').animate({
                        scrollTop: target.offset().top
                    }, 1000);
                    return false;
                }
            }
        });
    }
};

$("body").on("click", ".faq-group > header", function (t) {
    var n = $(this).next(".faq-content"); if (n.is(":hidden")) { n.slideDown() } else { n.slideUp() }
    if ($(this).children(".fa").hasClass("fa-plus"))
    { $(this).children(".fa").removeClass("fa-plus").addClass("fa-minus") } else if ($(this).children(".fa").hasClass("fa-minus"))
    { $(this).children(".fa").removeClass("fa-minus").addClass("fa-plus") }
    t.preventDefault();
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