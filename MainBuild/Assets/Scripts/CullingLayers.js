var layer9: float;
var layer10: float;
var layer11: float;

function Start () {
    var distances = new float[32];
    // Set up layer 10 to cull at 15 meters distance.
    // All other layers use the far clip plane distance.
    distances[9] = layer9;
    distances[10] = layer10;
    distances[11] = layer11;
    camera.layerCullDistances = distances;
}