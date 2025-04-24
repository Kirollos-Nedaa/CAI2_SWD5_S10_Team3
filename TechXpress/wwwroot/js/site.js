// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-link');

    navLinks.forEach(link => {
        // Remove trailing slashes for comparison
        const linkPath = new URL(link.href).pathname.replace(/\/$/, '');
        const normalizedCurrentPath = currentPath.replace(/\/$/, '');

        // Check if current path starts with link path (for nested routes)
        if (normalizedCurrentPath.startsWith(linkPath) && linkPath !== '/') {
            link.classList.add('text-black');
            link.classList.remove('text-gray-400');
        }
        // Special case for home page (exact match)
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