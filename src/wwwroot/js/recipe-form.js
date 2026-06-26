(function () {
    var ingredientsInput = document.getElementById('IngredientsJson');
    var stepsInput = document.getElementById('StepsJson');
    var ingredientsList = document.getElementById('ingredientsList');
    var stepsList = document.getElementById('stepsList');
    var form = document.getElementById('recipeForm');
    var thumbnailInput = document.getElementById('thumbnailFileInput');
    var thumbnailPreview = document.getElementById('thumbnailPreview');
    var thumbnailPlaceholder = document.getElementById('thumbnailPreviewPlaceholder');

    if (!ingredientsInput || !stepsInput || !ingredientsList || !stepsList) return;

    var inputClass = 'w-full px-3 py-2 rounded-lg border border-outline-variant bg-surface-container-lowest text-sm outline-none focus:border-primary-container';

    function parseJson(value, fallback) {
        try {
            return JSON.parse(value || JSON.stringify(fallback));
        } catch {
            return fallback;
        }
    }

    function syncIngredients() {
        var items = [];
        ingredientsList.querySelectorAll('[data-ingredient-item]').forEach(function (row) {
            items.push({
                name: row.querySelector('[data-field="name"]').value.trim(),
                quantity: row.querySelector('[data-field="quantity"]').value.trim()
            });
        });
        ingredientsInput.value = JSON.stringify(items);
    }

    function syncSteps() {
        var items = [];
        stepsList.querySelectorAll('[data-step-item]').forEach(function (row, index) {
            items.push({
                order: parseInt(row.querySelector('[data-field="order"]').value, 10) || index + 1,
                title: row.querySelector('[data-field="title"]').value.trim(),
                description: row.querySelector('[data-field="description"]').value.trim()
            });
        });
        stepsInput.value = JSON.stringify(items);
    }

    function createIngredientRow(item) {
        item = item || { name: '', quantity: '' };
        var row = document.createElement('div');
        row.className = 'flex flex-col sm:flex-row gap-2 items-start sm:items-center border border-outline-variant rounded-lg p-3 bg-surface-container-low/30';
        row.setAttribute('data-ingredient-item', '');

        row.innerHTML =
            '<input data-field="name" type="text" placeholder="Tên nguyên liệu (vd: Thịt ba chỉ)" class="' + inputClass + ' flex-1" value="' + escapeAttr(item.name) + '">' +
            '<input data-field="quantity" type="text" placeholder="Số lượng (vd: 500g)" class="' + inputClass + ' sm:w-40" value="' + escapeAttr(item.quantity) + '">' +
            '<button type="button" data-remove class="shrink-0 w-8 h-8 flex items-center justify-center rounded-lg text-red-500 hover:bg-red-50" title="Xóa">' +
            '<span class="material-symbols-outlined text-[18px]">close</span></button>';

        row.querySelector('[data-remove]').addEventListener('click', function () {
            row.remove();
            syncIngredients();
        });
        row.querySelectorAll('input').forEach(function (input) {
            input.addEventListener('input', syncIngredients);
        });

        return row;
    }

    function createStepRow(item, index) {
        item = item || { order: index + 1, title: '', description: '' };
        var row = document.createElement('div');
        row.className = 'border border-outline-variant rounded-lg p-3 bg-surface-container-low/30 space-y-2';
        row.setAttribute('data-step-item', '');

        row.innerHTML =
            '<div class="flex gap-2 items-center">' +
            '<input data-field="order" type="number" min="1" class="' + inputClass + ' w-20" value="' + (item.order || index + 1) + '">' +
            '<input data-field="title" type="text" placeholder="Tiêu đề bước (vd: Sơ chế)" class="' + inputClass + ' flex-1" value="' + escapeAttr(item.title) + '">' +
            '<button type="button" data-remove class="shrink-0 w-8 h-8 flex items-center justify-center rounded-lg text-red-500 hover:bg-red-50" title="Xóa">' +
            '<span class="material-symbols-outlined text-[18px]">close</span></button></div>' +
            '<textarea data-field="description" rows="2" placeholder="Mô tả chi tiết bước nấu..." class="' + inputClass + ' w-full resize-y">' + escapeHtml(item.description) + '</textarea>';

        row.querySelector('[data-remove]').addEventListener('click', function () {
            row.remove();
            syncSteps();
        });
        row.querySelectorAll('input, textarea').forEach(function (input) {
            input.addEventListener('input', syncSteps);
        });

        return row;
    }

    function escapeAttr(str) {
        return String(str || '').replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/</g, '&lt;');
    }

    function escapeHtml(str) {
        return String(str || '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }

    function renderIngredients() {
        ingredientsList.innerHTML = '';
        var items = parseJson(ingredientsInput.value, []);
        if (!items.length) items = [{ name: '', quantity: '' }];
        items.forEach(function (item) {
            ingredientsList.appendChild(createIngredientRow(item));
        });
        syncIngredients();
    }

    function renderSteps() {
        stepsList.innerHTML = '';
        var items = parseJson(stepsInput.value, []);
        if (!items.length) items = [{ order: 1, title: '', description: '' }];
        items.forEach(function (item, index) {
            stepsList.appendChild(createStepRow(item, index));
        });
        syncSteps();
    }

    document.getElementById('addIngredientBtn').addEventListener('click', function () {
        ingredientsList.appendChild(createIngredientRow());
        syncIngredients();
    });

    document.getElementById('addStepBtn').addEventListener('click', function () {
        var index = stepsList.querySelectorAll('[data-step-item]').length;
        stepsList.appendChild(createStepRow({ order: index + 1, title: '', description: '' }, index));
        syncSteps();
    });

    if (form) {
        form.addEventListener('submit', function () {
            syncIngredients();
            syncSteps();
        });
    }

    if (thumbnailInput) {
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
    }

    renderIngredients();
    renderSteps();
})();
