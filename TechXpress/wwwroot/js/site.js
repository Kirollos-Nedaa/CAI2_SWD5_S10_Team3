// Function to handle the active state of the navigation links
document.addEventListener('DOMContentLoaded', function () {
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-link');

    navLinks.forEach(link => {
        const linkPath = new URL(link.href).pathname.replace(/\/$/, '');
        const normalizedCurrentPath = currentPath.replace(/\/$/, '');

        if (normalizedCurrentPath.startsWith(linkPath) && linkPath !== '/') {
            link.classList.add('text-black');
            link.classList.remove('text-gray-400');
        }
        else if (linkPath === '/' && normalizedCurrentPath === '/') {
            link.classList.add('text-black');
            link.classList.remove('text-gray-400');
        }
    });
});

// Function to handle the click event on the button
document.addEventListener('DOMContentLoaded', function () {
    const mobileMenuButton = document.getElementById('mobile-menu-button');
    const mobileMenu = document.getElementById('mobile-menu');

    mobileMenuButton.addEventListener('click', function () {
        mobileMenu.classList.toggle('hidden');
    });
});

// Aside Script
const toggles = document.querySelectorAll('[data-toggle]');
const icons = document.querySelectorAll('[data-icon]');

toggles.forEach(button => {
    button.addEventListener('click', () => {
        const targetId = button.getAttribute('data-toggle');
        const target = document.getElementById(targetId);
        const icon = document.querySelector(`[data-icon="${targetId}"]`);

        document.querySelectorAll('#filterAccordion > div > div[id]').forEach(section => {
            const isCurrent = section.id === targetId;

            // Collapse all
            section.style.height = '0px';
            section.classList.remove('open');
            document.querySelector(`[data-icon="${section.id}"]`)?.classList.remove('rotate-180');

            if (isCurrent && !section.classList.contains('open')) {
                // Expand only selected
                section.classList.add('open');
                const inner = section.firstElementChild;
                section.style.height = inner.scrollHeight + 'px';
                icon?.classList.add('rotate-180');
            }
        });
    });
});

// Optional: reset height after transition so it adapts to dynamic content
document.querySelectorAll('#filterAccordion > div > div[id]').forEach(panel => {
    panel.addEventListener('transitionend', () => {
        if (panel.classList.contains('open')) {
            panel.style.height = 'auto';
        }
    });
});

// Update price text
const slider = document.getElementById("priceSlider");
const priceValue = document.getElementById("priceValue");
slider.addEventListener("input", () => {
    priceValue.textContent = slider.value;
});

// Apply & Clear buttons logic

/*document.getElementById('applyFilters').addEventListener('click', function () {
    // TODO: Trigger filtering logic
    console.log("Filters applied");
});

document.getElementById('clearFilters').addEventListener('click', function () {
    // Uncheck all checkboxes
    document.querySelectorAll('#filterAccordion input[type="checkbox"]').forEach(cb => cb.checked = false);

    // Reset slider
    const slider = document.getElementById('priceSlider');
    slider.value = 500;
    document.getElementById('priceValue').textContent = slider.value;

    // Optionally, re-apply filter logic
    console.log("Filters cleared");
});*/