// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


    // Läs in sparat tema vid sidladdning
    const savedTheme = localStorage.getItem('theme') || 'light';
    document.documentElement.setAttribute('data-bs-theme', savedTheme);
    updateToggleButton(savedTheme);

    document.getElementById('themeToggle').addEventListener('click', () => {
        const current = document.documentElement.getAttribute('data-bs-theme');
    const next = current === 'dark' ? 'light' : 'dark';
    document.documentElement.setAttribute('data-bs-theme', next);
    localStorage.setItem('theme', next);
    updateToggleButton(next);
    });

    function updateToggleButton(theme) {
        const btn = document.getElementById('themeToggle');
    btn.textContent = theme === 'dark' ? '☀️ Light mode' : '🌙 Dark mode';
    }
