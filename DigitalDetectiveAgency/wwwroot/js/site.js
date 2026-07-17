// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Global scripts for Digital Detective Agency
document.addEventListener("DOMContentLoaded", () => {
    console.log("🕵️ Detective Agency UI Engine fully loaded.");

    // Auto-fade alert banners if you use TempData for success/failure alerts
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.classList.remove('show');
            setTimeout(() => alert.remove(), 150);
        }, 4000);
    });
});