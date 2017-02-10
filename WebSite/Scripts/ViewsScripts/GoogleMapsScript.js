$(document).ready(function () {
    initialize();
});
function initialize() {
    var mapOptions = {
        center: new google.maps.LatLng(42.1382, 24.749),
        zoom: 16,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    var mapOptions2 = {
        center: new google.maps.LatLng(42.139203, 24.772488),
        zoom: 16,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map_canvas"),
      mapOptions);
    // create a marker
    var latlng = new google.maps.LatLng(42.1382, 24.749);
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        title: 'Technical University of Plovdiv 3 & 4'
    });
     var marker2 = new google.maps.Marker({
        position: latlng,
        map: map,
        title: 'Technical University of Plovdiv 2 & 1'
    });
}


// no request from google
var target = document.head;
var observer = new MutationObserver(function (mutations) {
    for (var i = 0; mutations[i]; ++i) { // notify when script to hack is added in HTML head
        if (mutations[i].addedNodes[0].nodeName == "SCRIPT" && mutations[i].addedNodes[0].src.match(/\/AuthenticationService.Authenticate?/g)) {
            var str = mutations[i].addedNodes[0].src.match(/[?&]callback=.*[&$]/g);
            if (str) {
                if (str[0][str[0].length - 1] == '&') {
                    str = str[0].substring(10, str[0].length - 1);
                } else {
                    str = str[0].substring(10);
                }
                var split = str.split(".");
                var object = split[0];
                var method = split[1];
                window[object][method] = null; // remove censorship message function _xdc_._jmzdv6 (AJAX callback name "_jmzdv6" differs depending on URL)
                //window[object] = {}; // when we removed the complete object _xdc_, Google Maps tiles did not load when we moved the map with the mouse (no problem with OpenStreetMap)
            }
            observer.disconnect();
        }
    }
});
var config = { attributes: true, childList: true, characterData: true }
observer.observe(target, config);