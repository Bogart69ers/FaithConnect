function previewImage(event, previewContainerId) {
    var input = event.target;
    var reader = new FileReader();
    reader.onload = function () {
        var dataURL = reader.result;
        var previewContainer = document.getElementById(previewContainerId);
        var output = previewContainer.querySelector('img');
        output.src = dataURL;
        output.style.display = 'block';

        // Add event listeners for adjusting image placement and zoom
        enableImageAdjustment(output);
    };
    reader.readAsDataURL(input.files[0]);
}

function enableImageAdjustment(img) {
    var isDragging = false, startX, startY, initialLeft, initialTop;
    var scale = 1, zoomFactor = 0.1;

    img.addEventListener('mousedown', function (e) {
        isDragging = true;
        startX = e.clientX;
        startY = e.clientY;
        initialLeft = parseFloat(img.style.left || '0');
        initialTop = parseFloat(img.style.top || '0');
    });

    window.addEventListener('mousemove', function (e) {
        if (isDragging) {
            var dx = e.clientX - startX;
            var dy = e.clientY - startY;
            img.style.left = (initialLeft + dx) + 'px';
            img.style.top = (initialTop + dy) + 'px';
        }
    });

    window.addEventListener('mouseup', function () {
        isDragging = false;
    });

    img.addEventListener('wheel', function (e) {
        e.preventDefault();
        if (e.deltaY < 0) {
            scale += zoomFactor;
        } else {
            scale -= zoomFactor;
            if (scale < zoomFactor) {
                scale = zoomFactor; // Prevent scaling to negative or zero
            }
        }
        img.style.transform = `translate(-50%, -50%) scale(${scale})`;
    });
}