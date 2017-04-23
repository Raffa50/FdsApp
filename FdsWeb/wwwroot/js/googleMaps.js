var map;

function initMap(createEvent) {
    navigator.geolocation.getCurrentPosition( loc => 
        createGMaps(new google.maps.LatLng( loc.coords.latitude,  loc.coords.longitude), createEvent),
        err => createGMaps( new google.maps.LatLng(44.40678, 8.93391)), createEvent );
}

function createGMaps(position, createEvent= true) {
    var mapProp = {
        center: position,
        zoom: 5,
        streetViewControl: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

    if ( createEvent ) { //crea evento alla posizione cliccata

    } else {
        setInterval(() => updatePosition, 3000);
    }
}

function updatePosition() {
    navigator.geolocation.getCurrentPosition( loc => 
        map.setCenter(new google.maps.LatLng( loc.coords.latitude,  loc.coords.longitude))
    );
}