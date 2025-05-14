document.addEventListener('DOMContentLoaded', function () {
    // Active navigation links
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

    // Mobile menu toggle
    const mobileMenuButton = document.getElementById('mobile-menu-button');
    const mobileMenu = document.getElementById('mobile-menu');

    if (mobileMenuButton && mobileMenu) {
        mobileMenuButton.addEventListener('click', function () {
            mobileMenu.classList.toggle('hidden');
        });
    }

    // Aside Script (accordion functionality)
    const toggles = document.querySelectorAll('[data-toggle]');

    if (toggles.length > 0) {
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
    }

    // Price slider update
    const slider = document.getElementById("priceSlider");
    const priceValue = document.getElementById("priceValue");

    if (slider && priceValue) {
        slider.addEventListener("input", () => {
            priceValue.textContent = slider.value;
        });
    }

    // User dropdown functionality
    function setupDropdown(buttonId, dropdownId) {
        const button = document.getElementById(buttonId);
        const dropdown = document.getElementById(dropdownId);

        if (!button || !dropdown) return;

        button.addEventListener('click', function (e) {
            e.stopPropagation();
            dropdown.classList.toggle('hidden');
        });

        document.addEventListener('click', function () {
            dropdown.classList.add('hidden');
        });

        dropdown.addEventListener('click', function (e) {
            e.stopPropagation();
        });
    }

    setupDropdown('userMenuButton', 'userDropdown');
    setupDropdown('mobileUserButton', 'mobileUserDropdown');
});

// Quantity buttons
function incrementQuantity(button) {
    const input = button.parentElement.querySelector('input[type="number"]');
    if (input) {
        input.value = parseInt(input.value) + 1;
        input.dispatchEvent(new Event('change'));
    }
}

function decrementQuantity(button) {
    const input = button.parentElement.querySelector('input[type="number"]');
    if (input && parseInt(input.value) > 1) {
        input.value = parseInt(input.value) - 1;
        input.dispatchEvent(new Event('change'));
    }
}