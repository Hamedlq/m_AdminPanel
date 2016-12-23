
/*----------------------------------------------------*/
/* Google javascript api v3  -- map */
/*----------------------------------------------------*/
(function () {
    var distributionPoints = [];
    var markersArray = [];
    var counterVal = 0;
    var map;
    function initialize() {
        /* change your with your coordinates */
        var myLatLng = new google.maps.LatLng(35.716900820, 51.426422596), // Your coordinates
            mappy = {
                center: myLatLng,
                zoom: 15,
                scrollwheel: false,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
        map= new google.maps.Map(document.getElementById("map"), mappy);

        google.maps.event.addListener(map, "click", function (event) {
            placeMarker(event.latLng, map);
        });

        //marker=new google.maps.Marker({
        //    position: myLatLng,
        //    map: map,
        //    animation: google.maps.Animation.DROP,
        //    title: "مکان دقیق رویداد" // Title for marker
        //});
    }

    function clearOverlays() {
        for (var i = 0; i < markersArray.length; i++) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }

    if (document.getElementById("map")) {
        google.maps.event.addDomListener(window, "load", initialize);

    }

    function placeMarker(location, map) {
        /*if (marker !== undefined && marker !== null) {
            marker.setMap(null);
        }*/
        $("#Latitude").val(location.lat());
        $("#Longitude").val(location.lng());
        distributionPoints.push({
            counter: counterVal,
            lat: location.lat(),
            lng: location.lng()
        });
        counterVal++;
        var marker = new google.maps.Marker({
            position: location,
            map: map,
            animation: google.maps.Animation.DROP,
            title: "مکان تحویل" // Title for marker
        });
        markersArray.push(marker);
    }


    $('#distributeForm').submit(function(event) {
        var $this = $(this);
        event.preventDefault();
        $('.distribute-alert-wrap').children().remove();
        $(".distribute-alert-wrap").append('<div class="alert alert-info">لطفا شکیبا باشید...</div>');
        $.ajax({
            dataType: 'json',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(distributionPoints),
            url: '/Distribute/insert',
            success: function(data) {
                if (data.Errors === undefined || data.Errors === null || data.Errors.length === 0) {
                    $('.distribute-alert-wrap').children().remove();
                    initialize();
                    distributionPoints.length = 0;
                    clearOverlays();
                    $('.distribute-alert-wrap').append('<div class="alert alert-success">رفت</div>');
                    $.each(data.Messages, function (key, value) {
                        var distributeResponse = JSON.parse(value);
                        $.each(distributeResponse.DistributeRoute, function (dkey, dvalue) {
                            var distributeRoute = dvalue;
                            var marker = new google.maps.Marker({
                                position: new google.maps.LatLng(distributeRoute.PickUpPoint.Lat, distributeRoute.PickUpPoint.Lng),
                                map: map,
                                title: "مکان جدید تحویل" // Title for marker
                            });
                            markersArray.push(marker);
                            $.each(distributeRoute.DeliverRoutes, function(dlkey, dlvalue) {
                                var deliverRoute = dlvalue;
                                var line = new google.maps.Polyline({
                                    path: [
                                        new google.maps.LatLng(deliverRoute.Start.Lat, deliverRoute.Start.Lng),
                                        new google.maps.LatLng(deliverRoute.End.Lat, deliverRoute.End.Lng)
                                    ],
                                    strokeColor: "#FF0000",
                                    strokeOpacity: 1.0,
                                    strokeWeight: 10,
                                    map: map
                                });
                            });

                        });

                    });
                }
            }
        }).error(function(error) {
            $('.distribute-alert-wrap').children().remove();
            $('.distribute-alert-wrap').append('<div class="alert alert-danger"><strong>خطا : </strong>خطا در ارتباط</div>');
        });
    });

}());
