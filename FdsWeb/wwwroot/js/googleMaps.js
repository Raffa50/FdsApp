var map, marker, markers= [], geocoder= new google.maps.Geocoder();

function initMap(createEvent, cb) {
    navigator.geolocation.getCurrentPosition( loc => 
        createGMaps( new google.maps.LatLng( loc.coords.latitude,  loc.coords.longitude), createEvent, cb ),
        err => createGMaps(new google.maps.LatLng(44.40678, 8.93391), createEvent, cb)
    );
}

function createGMaps(position, createEvent= false, cb) {
    var mapProp = {
        center: position,
        zoom: 5,
        streetViewControl: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

    if( createEvent ) { //crea evento alla posizione cliccata
        var input = $("#pac-input");
        var searchBox = new google.maps.places.SearchBox(input.get(0));

        searchBox.addListener("places_changed", () => {
                var places = searchBox.getPlaces();

                if (places.length == 0)
                    return;

                placeMarker(places[0].geometry.location, places[0].title);
            });

        //address check
        input.change(checkValidAddress);

        google.maps.event.addListener(map, "click", event => {
                placeMarker(event.latLng);
                geocodePosition(event.latLng);
        });

        // Resize stuff...
        google.maps.event.addDomListener(window, "resize", () => {
            var center = map.getCenter();
            google.maps.event.trigger(map, "resize");
            map.setCenter(center);
        });
    } else {
        setInterval(() => updatePosition, 3000);
    }

    if( cb != null )
        cb();
}

function placeMarker(location, title) {
    if (marker != null)
        marker.setMap(null); //eliminare vecchio marker

    marker = new google.maps.Marker({
        position: location,
        title: title,
        map: map
    });

    map.setCenter(location);
}

function addEventMaker(location, label) {
    markers[label] = new google.maps.Marker({
        position: location,
        label: label,
        map: map
    });
}

function checkValidAddress() {
    // Geocode the address
    geocoder.geocode({ 'address': $("#pac-input").val() }, (results, status) => {
        if (status === google.maps.GeocoderStatus.OK && results.length > 0) {
            // set it ot he correct, formatted address if it's vadlid
            $("#pac-input").val(results[0].formatted_address);
            validAddress = true;
        } else
            validAddress = false;
    });
}

function geocodePosition(pos) {
    geocoder.geocode({
        latLng: pos
    }, function (responses) {
        if (responses && responses.length > 0) {
            $("#pac-input").val(responses[0].formatted_address);
        }
    });
}

function updatePosition() {
    navigator.geolocation.getCurrentPosition( loc => 
        map.setCenter(new google.maps.LatLng( loc.coords.latitude,  loc.coords.longitude))
    );
}