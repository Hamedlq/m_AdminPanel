var map, featureList, boroughSearch = [], theaterSearch = [], museumSearch = [];
var appointMarker = false;
function newRouteMenu() {
    $('#address-container').show();
    initializeNewRoute();
    $("#feature-list tbody").empty();
}
//-----------Notification--------------
var isModalOpen = function () {
    return ($(".modal.in").data('bs.modal') || {}).isShown;
}
function showInModal(msg) {
    var thisModalAlertWrapper = $(".modal.in").find('.alert-wrap');
    thisModalAlertWrapper.append(msg);
}

function showinGrowl(type, title, msg) {
    $.notify({ title: title, message: msg }, { type: type, placement: { from: "top", align: "left" }, offset: { x: 20, y: 50 }, delay: 5000, speed: 600, maxOpen: 3 });
    return false;
}

var showMessage = {
    Error: function (title, msg) {
        var message = '<div class="alert alert-block alert-danger"><button class="close pull-left" data-dismiss="alert" type="button">×</button><strong>' + title + '</strong>' + msg + '</div>';
        if (isModalOpen()) {
            showInModal(message);
        } else {
            showinGrowl("danger", title, msg);
        }
    },
    Warning: function (title, msg) {
        var message = '<div class="alert alert-block alert-warning"><button class="close pull-left" data-dismiss="alert" type="button">×</button><strong>' + title + '</strong>' + msg + '</div>';
        if (isModalOpen()) {
            showInModal(message);
        } else {
            showinGrowl("warning", title, msg);
        }
    },
    Info: function (title, msg) {
        var message = '<div class="alert alert-block alert-info"><button class="close pull-left" data-dismiss="alert" type="button">×</button><strong>' + title + '</strong>' + msg + '</div>';
        if (isModalOpen()) {
            showInModal(message);
        } else {
            showinGrowl("info", title, msg);
        }
    },
    Success: function (title, msg) {
        var message = '<div class="alert alert-block alert-success"><button class="close pull-left" data-dismiss="alert" type="button">×</button><strong>' + title + '</strong>' + msg + '</div>';
        if (isModalOpen()) {
            showInModal(message);
        } else {
            showinGrowl("success", title, msg);
        }
    }
}
//----------End Notification--------------
newRouteMenu();

$(window).resize(function () {
    sizeLayerControl();
});

$("#full-extent-btn").click(function () {
    map.fitBounds(boroughs.getBounds());
    $(".navbar-collapse.in").collapse("hide");
    return false;
});


$(document).on("click", "#timingItem,#timingMenuItem", function () {
    $("#timingModal").modal("show");
    return false;
});
$(document).on("click", "#priceItem,#priceMenuItem", function () {
    $("#pricingModal").modal("show");
});
$(document).on("click", "#confirmItem,#confirmMenuItem", function () {
    $("#confirmModal").modal("show");
});


$("#rlist-btn").click(function () {
    $('#sidebar-right').toggle();
    map.invalidateSize();
    return false;
});
$("#llist-btn").click(function () {
    $('#sidebar-left').toggle();
    map.invalidateSize();
    return false;
});

$("#nav-btn").click(function () {
    $(".navbar-collapse").collapse("toggle");
    return false;
});

$("#sidebar-toggle-btn").click(function () {
    $("#sidebar-left").toggle();
    map.invalidateSize();
    return false;
});

$("#sidebar-hide-btn").click(function () {
    $('#sidebar-left').hide();
    map.invalidateSize();
});

$("#left_sidebar-hide-btn").click(function () {
    $('#sidebar-left').hide();
    map.invalidateSize();
});

function sizeLayerControl() {
    $(".leaflet-control-layers").css("max-height", $("#map").height() - 50);
}

function clearHighlight() {
    highlight.clearLayers();
}

function sidebarClick(id) {
    var layer = markerClusters.getLayer(id);
    map.setView([layer.getLatLng().lat, layer.getLatLng().lng], 17);
    layer.fire("click");
    /* Hide sidebar and go to the map on small screens */
    if (document.body.clientWidth <= 767) {
        $("#sidebar-left").hide();
        $("#sidebar-right").hide();
        $(".address-container").addClass("address-container-small");
        map.invalidateSize();
    }
}

/* Basemap Layers */
var cartoLight = L.tileLayer("https://cartodb-basemaps-{s}.global.ssl.fastly.net/light_all/{z}/{x}/{y}.png", {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, &copy; <a href="https://cartodb.com/attributions">CartoDB</a>'
});
var usgsImagery = L.layerGroup([L.tileLayer("http://basemap.nationalmap.gov/arcgis/rest/services/USGSImageryOnly/MapServer/tile/{z}/{y}/{x}", {
    maxZoom: 15,
}), L.tileLayer.wms("https://raster.nationalmap.gov/arcgis/services/Orthoimagery/USGS_EROS_Ortho_SCALE/ImageServer/WMSServer?", {
    minZoom: 16,
    maxZoom: 19,
    layers: "0",
    format: 'image/jpeg',
    transparent: true,
    attribution: "Aerial Imagery courtesy USGS"
})]);
var mapquestOSM = L.tileLayer("http://{s}.mqcdn.com/tiles/1.0.0/osm/{z}/{x}/{y}.png", {
    maxZoom: 19,
    subdomains: ["otile1", "otile2", "otile3", "otile4"],
    attribution: 'Tiles courtesy of <a href="http://www.mapquest.com/" target="_blank">MapQuest</a> <img src="http://developer.mapquest.com/content/osm/mq_logo.png">. Map data (c) <a href="http://www.openstreetmap.org/" target="_blank">OpenStreetMap</a> contributors, CC-BY-SA.'
});
var mapquestOAM = L.tileLayer("http://{s}.mqcdn.com/tiles/1.0.0/sat/{z}/{x}/{y}.jpg", {
    maxZoom: 18,
    subdomains: ["oatile1", "oatile2", "oatile3", "oatile4"],
    attribution: 'Tiles courtesy of <a href="http://www.mapquest.com/" target="_blank">MapQuest</a>. Portions Courtesy NASA/JPL-Caltech and U.S. Depart. of Agriculture, Farm Service Agency'
});
var mapquestHYB = L.layerGroup([L.tileLayer("http://{s}.mqcdn.com/tiles/1.0.0/sat/{z}/{x}/{y}.jpg", {
    maxZoom: 18,
    subdomains: ["oatile1", "oatile2", "oatile3", "oatile4"]
}), L.tileLayer("http://{s}.mqcdn.com/tiles/1.0.0/hyb/{z}/{x}/{y}.png", {
    maxZoom: 19,
    subdomains: ["oatile1", "oatile2", "oatile3", "oatile4"],
    attribution: 'Labels courtesy of <a href="http://www.mapquest.com/" target="_blank">MapQuest</a> <img src="http://developer.mapquest.com/content/osm/mq_logo.png">. Map data (c) <a href="http://www.openstreetmap.org/" target="_blank">OpenStreetMap</a> contributors, CC-BY-SA. Portions Courtesy NASA/JPL-Caltech and U.S. Depart. of Agriculture, Farm Service Agency'
})]);

/* Single marker cluster layer to hold all clusters */
var markerClusters = new L.MarkerClusterGroup({
    spiderfyOnMaxZoom: true,
    showCoverageOnHover: false,
    zoomToBoundsOnClick: true,
    disableClusteringAtZoom: 16
});

map = L.map("map", {
    zoom: 12,
    center: [35.7092593, 51.4489394],
    layers: [cartoLight, markerClusters],
    zoomControl: false,
    attributionControl: false
});

/* Attribution control */
function updateAttribution(e) {
    $.each(map._layers, function (index, layer) {
        if (layer.getAttribution) {
            $("#attribution").html((layer.getAttribution()));
        }
    });
}
map.on("layeradd", updateAttribution);
map.on("layerremove", updateAttribution);



var attributionControl = L.control({
    position: "bottomright"
});
attributionControl.onAdd = function (map) {
    var div = L.DomUtil.create("div", "leaflet-control-attribution");
    div.innerHTML = "<span class='hidden-xs'><a href='#'>Mibarim.com</a>";
    return div;
};
map.addControl(attributionControl);

var zoomControl = L.control.zoom({
    position: "topleft"
}).addTo(map);

/* GPS enabled geolocation control set to follow the user's location */
var locateControl = L.control.locate({
    position: "topleft",
    drawCircle: true,
    follow: true,
    setView: true,
    keepCurrentZoomLevel: true,
    markerStyle: {
        weight: 1,
        opacity: 0.8,
        fillOpacity: 0.8
    },
    circleStyle: {
        weight: 1,
        clickable: false
    },
    icon: "fa fa-location-arrow",
    metric: false,
    strings: {
        title: "مکان من",
        popup: "فاصله شما {distance} {unit}",
        outsideMapBoundsMsg: "مکان شما خارج نقشه است"
    },
    locateOptions: {
        maxZoom: 18,
        watch: true,
        enableHighAccuracy: true,
        maximumAge: 10000,
        timeout: 10000
    }
}).addTo(map);

map.locate({ setView: true, watch: false, maxZoom: 15 }) /* This will return map so you can do chaining */
       .on('locationfound', function (e) {
           setFlags(e);
           //var marker = L.marker([e.latitude, e.longitude]).bindPopup('Your are here :)');
           //var circle = L.circle([e.latitude, e.longitude], e.accuracy / 2, {
           //    weight: 1,
           //    color: 'blue',
           //    fillColor: '#cacaca',
           //    fillOpacity: 0.2
           //});
           //map.addLayer(marker);
           //map.addLayer(circle);
       })
      .on('locationerror', function (e) {
          console.log(e);
      });


/* Larger screens get expanded layer control and visible sidebar */
if (document.body.clientWidth <= 767) {
    var isCollapsed = true;
} else {
    var isCollapsed = false;
}

var baseLayers = {
    "نقشه خیابان": cartoLight,
    "نقشه ماهواره ای": usgsImagery
//    ,
//    "خیابان و ماهواره": mapquestHYB
};

$(document).one("ajaxStop", function () {
    $("#loading").hide();
});
var groupedOverlays = {};

var layerControl = L.control.groupedLayers(baseLayers, groupedOverlays, {
    collapsed: isCollapsed,
    position: "bottomright"
}).addTo(map);


// Leaflet patch to make layer control scrollable on touch browsers
var container = $(".leaflet-control-layers")[0];
if (!L.Browser.touch) {
    L.DomEvent
    .disableClickPropagation(container)
    .disableScrollPropagation(container);
} else {
    L.DomEvent.disableClickPropagation(container);
}

function activeNextTab(tab) {
    $('.nav-tabs a[href="#' + tab + '"]').tab('show');
};


function initializeNewRoute() {
    // ترتیب دکمه های صفحه اول
    $('#sourceDetail').hide();
    $('#destinationDetail').hide();
    srcStep();
}

function srcStep() {
    $('#dstAdd').hide();
    $('#srcAdd-btn').hide();
    $('#address-btn').hide();
    $("#dstAdd-btn").show();
    $("#srcAdd").show();
}
function dstStep() {
    $('#srcAdd-btn').show();
    $('#dstAdd').show();
    $('#address-btn').show();
    $("#dstAdd-btn").hide();
    $("#srcAdd").hide();
}

$("#dstAdd-btn").click(function (e) {
    if ($("#srclatlng").val() === "") {
        showMessage.Error("", $srcClickonMap);
    } else {
        dstStep();
    }
    e.stopPropagation();
    return false;
});
$("#srcAdd-btn").click(function (e) {
    srcStep();
    e.stopPropagation();
    return false;

});

$("#srcDetailBtn").click(function (e) {
    if ($('#sourceDetail').is(':visible')) {
        $('#sourceDetail').hide();
    } else {
        $('#sourceDetail').show();
    }
    e.stopPropagation();
    return false;
});
$("#dstDetailBtn").click(function (e) {
    if ($(' #destinationDetail').is(':visible')) {
        $('#destinationDetail').hide();
    } else {
        $('#destinationDetail').show();
    }
    e.stopPropagation();
    return false;
});

$("#timepickertoday").timepicker();
$("#timepickerspec").timepicker();
$("#sattimepicker").timepicker();
$("#suntimepicker").timepicker();
$("#montimepicker").timepicker();
$("#tuetimepicker").timepicker();
$("#wedtimepicker").timepicker();
$("#thutimepicker").timepicker();
$("#fritimepicker").timepicker();
$("#appointtimepicker").timepicker();
$(".select2").select2({ minimumResultsForSearch: 5 });

// http://maps.googleapis.com/maps/api/js/GeocodeService.Search?4s35.7092593%2C%2051.4489394&7sUS&9sfa&key=AIzaSyCJ1-2Zmo3__rSqbNmksDciZhVLy9u-jFs&callback=_xdc_._2iaon7&token=32679
var sourceMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconSrc });
var destinationMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconDst });
var appointmentMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconAppoint });
var srcSuggestMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconSrc });
var dstSuggestMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconDst });
var srcRedMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconRed });
var dstRedMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconRed });
var srcBlueMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconBlue });
var dstBlueMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconBlue });
var srcOrangeMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconOrange });
var dstOrangeMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconOrange });
var srcGreenMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconGreen });
var dstGreenMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconGreen });
var srcCarMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconCar });
var dstCarMarker = new L.Marker([35.7092593, 51.4489394], { icon: IconCar });

var IconSrc = L.icon({ iconUrl: "/Content/assets/img/From.png", iconAnchor: [25, 59] });
var IconDst = L.icon({ iconUrl: '/Content/assets/img/To.png', iconAnchor: [25, 59] });
var IconAppoint = L.icon({ iconUrl: '/Content/assets/img/appoint.png', iconAnchor: [25, 59] });
var IconRed = L.icon({ iconUrl: '/Content/assets/img/red-icon.png', iconSize: [25, 41], iconAnchor: [12, 41], popupAnchor: [1, -34], shadowSize: [41, 41] });
var IconBlue = L.icon({ iconUrl: '/Content/assets/img/blue-icon.png', iconSize: [25, 41], iconAnchor: [12, 41], popupAnchor: [1, -34], shadowSize: [41, 41] });
var IconOrange = L.icon({ iconUrl: '/Content/assets/img/orange-icon.png', iconSize: [25, 41], iconAnchor: [12, 41], popupAnchor: [1, -34], shadowSize: [41, 41] });
var IconGreen = L.icon({ iconUrl: '/Content/assets/img/green-icon.png', iconSize: [25, 41], iconAnchor: [12, 41], popupAnchor: [1, -34], shadowSize: [41, 41] });
var IconCar = L.icon({ iconUrl: '/Content/assets/img/car.png', iconSize: [45, 25], iconAnchor: [41, 5], popupAnchor: [-17, 1], shadowSize: [41, 41] });

map.on('click', setFlags);
function setFlags(e) {
    var srcdst = "";
    if (appointMarker === true) {
        setAppointmentPoint(e.latlng);
    } else {
        if ($('#srcAdd').is(':visible')) {
            srcMarker(e.latlng);
            clearSrc();
            srcdst = "src";
            $('#srclatlng').val(e.latlng.lat + "," + e.latlng.lng);
            getAdressfromg(e, srcdst);
        }
        else if ($('#dstAdd').is(':visible')) {
            dstMarker(e.latlng);
            clearDst();
            srcdst = "dst";
            $('#dstlatlng').val(e.latlng.lat + "," + e.latlng.lng);
            getAdressfromg(e, srcdst);
        }
    }
}

function getAdressfromg(e, srcdst) {
    $.ajax({
        url: 'https://maps.googleapis.com/maps/api/geocode/json?latlng=' + e.latlng.lat + ',' + e.latlng.lng + '&language=fa&location_type=GEOMETRIC_CENTER|APPROXIMATE&key=AIzaSyAbHHEaaGfcm2jtmfdbvu_qraFZAbr0QGM',
        //url:'http://nominatim.openstreetmap.org/reverse?accept-language=fa&format=json&lat='+e.latlng.lat+'&lon='+e.latlng.lng+'&zoom=18&addressdetails=1',
        success: function (data) {
            //if (data.status === "OK") {
                if (srcdst === "src") {
                    fillSrc(data.results);
                    //fillSrc(data.address);
                }
                if (srcdst === "dst") {
                    fillDst(data.results);
                }
            //}
        }
    }).error(function (error) {
        alert("خطا در ارتباط اینترنت، لطفا دوباره روی نقشه  کلیک کنید");
    });
}

function getAddressFromgResponse(data) {
    var route='';
    var neighborhood='';
    //var sublocality = '';
    var locality = '';
        $.each(data.address_components, function(key, value) {
            $.each(value.types, function(typekey, typevalue) {
                if (typevalue === "route") {
                    route = value.long_name;
                }
                if (typevalue === "neighborhood") {
                    neighborhood = value.long_name + '، ';
                }
                //if (typevalue === "sublocality") {
                //    sublocality = value.long_name + '، ';
                //}
                if (typevalue === "locality") {
                    locality = value.long_name + '، ';
                }
            });
        });
        return locality  + neighborhood + route;
}

function removeLayerFromMap(layer) {
    map.removeLayer(layer);
}

//function removeSrcMarker() {
//    map.removeLayer(sourceMarker);
//};
//function removeSuggestSrcMarker() {
//    map.removeLayer(srcSuggestMarker);
//};

//function removeDstMarker() {
//    map.removeLayer(destinationMarker);
//};
//function removeSuggestDstMarker() {
//    map.removeLayer(dstSuggestMarker);
//};

function srcMarker(latlng) {
    //removeSrcMarker();
    removeLayerFromMap(sourceMarker);
    sourceMarker = new L.Marker(latlng, { icon: IconSrc });
    map.addLayer(sourceMarker);
}
function dstMarker(latlng) {
    //removeDstMarker();
    removeLayerFromMap(destinationMarker);
    destinationMarker = new L.Marker(latlng, { icon: IconDst });
    map.addLayer(destinationMarker);
}
function clearSrc() {
    var srcSelect = $('#srcAdd').find('select');
    srcSelect.find('option:gt(0)').remove();
    srcSelect.val(srcSelect.find('option:first').val());
    $('#srcAdd').find('.select2-chosen').text(srcSelect.find('option:first').text());
}
function fillSrc(data) {
    var srcSelect = $('#srcAdd').find('select');
    //srcSelect.append($("<option></option>").text(data.city + "، " + data.suburb + "، " + data.road));
    $.each(data, function(key, value) {
        var address;
        if (key === 0) {
            address = getAddressFromgResponse(value);
        } else {
            address = value.formatted_address;
        }
        srcSelect.append($("<option></option>")
                .attr("value", key)
                .text(address));
    });
    $('#srcAdd').find('.select2-chosen').text(srcSelect.find('option:eq(1)').text());
    srcSelect.find('option:eq(1)').attr('selected', 'selected');
}
function clearDst() {
    var dstSelect = $('#dstAdd').find('select');
    dstSelect.find('option:gt(0)').remove();
    dstSelect.val(dstSelect.find('option:first').val());
    $('#dstAdd').find('.select2-chosen').text(dstSelect.find('option:first').text());
}
function fillDst(data) {
    var dstSelect = $('#dstAdd').find('select');
    $.each(data, function (key, value) {
        var address;
        if (key === 0) {
            address = getAddressFromgResponse(value);
        } else {
            address = value.formatted_address;
        }
        dstSelect
            .append($("<option></option>")
            .attr("value", key)
            .text(address));
    });
    $('#dstAdd').find('.select2-chosen').text(dstSelect.find('option:eq(1)').text());
    dstSelect.find('option:eq(1)').attr('selected', 'selected');
}

function hideNewRoute() {
    //removeSrcMarker();
    removeLayerFromMap(sourceMarker);
    removeLayerFromMap(destinationMarker);
    //removeDstMarker();
    $('#address-container').hide();
}


$("#pricingModal .form-group").on("click", function () {
    $(this).find("input:radio").attr("checked", "checked");
});
$("body").on("click", ".portlet-header .btn", function (t) {
    if ($(this).children().hasClass("fa-minus")) {
        $(this).parents(".portlet").find(".portlet-content").slideUp(200, function () { $(this).parents(".portlet").addClass("portlet-closed") });
    } else {
        $(this).parents(".portlet").removeClass("portlet-closed").find(".portlet-content").slideDown(200);
    }
    if ($(this).children().hasClass("fa-plus"))
    { $(this).children().removeClass("fa-plus").addClass("fa-minus") } else if ($(this).children().hasClass("fa-minus"))
    { $(this).children().removeClass("fa-minus").addClass("fa-plus") }
    t.preventDefault();
});
$("body").on("click", ".faq-group > header", function (t) {
    var n = $(this).next(".faq-content"); if (n.is(":hidden")) { n.slideDown() } else { n.slideUp() }
    if ($(this).children(".fa").hasClass("fa-plus"))
    { $(this).children(".fa").removeClass("fa-plus").addClass("fa-minus") } else if ($(this).children(".fa").hasClass("fa-minus"))
    { $(this).children(".fa").removeClass("fa-minus").addClass("fa-plus") }
    t.preventDefault();
});

$(".dropdown-toggle").dropdown();


