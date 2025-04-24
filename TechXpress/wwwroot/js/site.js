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