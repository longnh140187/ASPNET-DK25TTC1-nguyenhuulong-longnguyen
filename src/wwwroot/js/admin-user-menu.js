(function () {
    document.querySelectorAll('[data-admin-user-menu]').forEach(function (menu) {
        var toggle = menu.querySelector('[data-admin-user-menu-toggle]');
        var dropdown = menu.querySelector('[data-admin-user-menu-dropdown]');
        var chevron = menu.querySelector('[data-admin-user-menu-chevron]');

        if (!toggle || !dropdown) return;

        function closeMenu() {
            dropdown.classList.add('hidden');
            toggle.setAttribute('aria-expanded', 'false');
            if (chevron) chevron.style.transform = '';
        }

        function openMenu() {
            dropdown.classList.remove('hidden');
            toggle.setAttribute('aria-expanded', 'true');
            if (chevron) chevron.style.transform = 'rotate(180deg)';
        }

        toggle.addEventListener('click', function (e) {
            e.stopPropagation();
            var isOpen = !dropdown.classList.contains('hidden');
            if (isOpen) {
                closeMenu();
            } else {
                openMenu();
            }
        });

        document.addEventListener('click', function () {
            closeMenu();
        });

        dropdown.addEventListener('click', function (e) {
            e.stopPropagation();
        });
    });
})();
