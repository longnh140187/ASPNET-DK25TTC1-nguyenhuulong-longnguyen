(function () {
    var thumbnailInput = document.getElementById('thumbnailFileInput');
    var thumbnailPreview = document.getElementById('thumbnailPreview');
    var thumbnailPlaceholder = document.getElementById('thumbnailPreviewPlaceholder');

    if (!thumbnailInput) return;

    thumbnailInput.addEventListener('change', function () {
        var file = thumbnailInput.files && thumbnailInput.files[0];
        if (!file) return;

        var reader = new FileReader();
        reader.onload = function (e) {
            if (thumbnailPreview) {
                thumbnailPreview.src = e.target.result;
                thumbnailPreview.classList.remove('hidden');
            }
            if (thumbnailPlaceholder) {
                thumbnailPlaceholder.classList.add('hidden');
            }
        };
        reader.readAsDataURL(file);
    });
})();
