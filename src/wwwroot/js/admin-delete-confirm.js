(function () {
    document.querySelectorAll('[data-admin-delete-form]').forEach(function (form) {
        form.addEventListener('submit', function (event) {
            var message = form.getAttribute('data-confirm-message') || 'Bạn có chắc muốn xóa mục này?';
            if (!window.confirm(message)) {
                event.preventDefault();
            }
        });
    });
})();
