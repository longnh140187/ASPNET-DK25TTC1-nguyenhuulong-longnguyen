(function () {
    const PRESET_COLORS = [
        '#FF6B00', '#85DC5B', '#8B5CF6', '#EC4899', '#3B82F6',
        '#EF4444', '#F59E0B', '#14B8A6', '#6366F1', '#64748B'
    ];

    const toggleBtn = document.getElementById('colorPickerToggle');
    const palette = document.getElementById('colorPalette');
    const swatchesContainer = document.getElementById('colorSwatches');
    const colorInput = document.getElementById('ColorInput');
    const colorPreview = document.getElementById('colorPreview');
    const colorValue = document.getElementById('colorValue');
    const customColorInput = document.getElementById('customColorInput');
    const customColorHex = document.getElementById('customColorHex');

    if (!toggleBtn || !palette || !colorInput) return;

    function setColor(hex) {
        if (!hex) return;
        const normalized = hex.startsWith('#') ? hex.toUpperCase() : `#${hex.toUpperCase()}`;
        colorInput.value = normalized;
        colorPreview.style.backgroundColor = normalized;
        colorValue.textContent = normalized;
        customColorInput.value = normalized;
        customColorHex.value = normalized;
    }

    function renderSwatches() {
        swatchesContainer.innerHTML = '';
        PRESET_COLORS.forEach(function (hex) {
            const btn = document.createElement('button');
            btn.type = 'button';
            btn.title = hex;
            btn.className = 'w-10 h-10 rounded-lg border-2 border-transparent hover:border-primary-container hover:scale-105 transition-all';
            btn.style.backgroundColor = hex;
            btn.addEventListener('click', function () {
                setColor(hex);
                palette.classList.add('hidden');
            });
            swatchesContainer.appendChild(btn);
        });
    }

    toggleBtn.addEventListener('click', function () {
        palette.classList.toggle('hidden');
    });

    document.addEventListener('click', function (e) {
        if (!palette.contains(e.target) && !toggleBtn.contains(e.target)) {
            palette.classList.add('hidden');
        }
    });

    customColorInput.addEventListener('input', function () {
        setColor(customColorInput.value);
    });

    customColorHex.addEventListener('input', function () {
        const val = customColorHex.value.trim();
        if (/^#[0-9A-Fa-f]{6}$/.test(val)) {
            setColor(val);
        }
    });

    renderSwatches();

    if (colorInput.value) {
        setColor(colorInput.value);
    } else {
        colorValue.textContent = 'Chọn màu...';
    }
})();
