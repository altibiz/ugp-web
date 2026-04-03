function drawBarcodeToDiv(divId, textToEncode) {
    // Barcode generation sample copied from library sample
    PDF417.init(textToEncode);
    var barcode = PDF417.getBarcodeArray();
    // block sizes (width and height) in pixels
    var bw = 2;
    var bh = 2;
    // create canvas element based on number of columns and rows in barcode
    var container = document.getElementById(divId);
    if (container.firstChild) {
        container.removeChild(container.firstChild);
    }
    var canvas = document.createElement('canvas');
    canvas.width = bw * barcode['num_cols'];
    canvas.height = bh * barcode['num_rows'];
    container.appendChild(canvas);
    var ctx = canvas.getContext('2d');
    // graph barcode elements
    var y = 0;
    // for each row
    for (var r = 0; r < barcode['num_rows']; ++r) {
        var x = 0;
        // for each column
        for (var c = 0; c < barcode['num_cols']; ++c) {
            if (barcode['bcode'][r][c] == 1) {
                ctx.fillRect(x, y, bw, bh);
            }
            x += bw;
        }
        y += bh;
    }
}

function downloadBarcode(divId, amount) {
    var canvas = document.getElementById(divId).firstChild;
    if (canvas) {
        // Create a temporary canvas with white background
        var tempCanvas = document.createElement('canvas');
        tempCanvas.width = canvas.width;
        tempCanvas.height = canvas.height;
        var ctx = tempCanvas.getContext('2d');

        // Fill with white background
        ctx.fillStyle = '#FFFFFF';
        ctx.fillRect(0, 0, tempCanvas.width, tempCanvas.height);

        // Draw the barcode on top
        ctx.drawImage(canvas, 0, 0);

        // Download the image
        var link = document.createElement('a');
        link.download = `glas-poduzetnika-barkod-${amount}.png`;
        link.href = tempCanvas.toDataURL('image/png');
        link.click();
    }
}