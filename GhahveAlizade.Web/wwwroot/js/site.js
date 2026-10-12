// ==========================================================================
// GhahveAlizade - Global Scripts
// ==========================================================================

(function () {
    'use strict';

    // ============ Lazy Image Loader ============
    function initLazyImages() {
        if ('loading' in HTMLImageElement.prototype) {
            // Native lazy loading پشتیبانی می‌شه
            document.querySelectorAll('img[loading="lazy"]').forEach(img => {
                if (img.complete) img.classList.add('loaded');
                else img.addEventListener('load', () => img.classList.add('loaded'));
            });
        } else {
            // Fallback با IntersectionObserver
            const observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        const img = entry.target;
                        if (img.dataset.src) img.src = img.dataset.src;
                        img.classList.add('loaded');
                        observer.unobserve(img);
                    }
                });
            }, { rootMargin: '50px' });

            document.querySelectorAll('img[data-src]').forEach(img => observer.observe(img));
        }
    }

    // ============ Confirm Delete ============
    function initConfirmDelete() {
        document.querySelectorAll('[data-confirm]').forEach(el => {
            el.addEventListener('click', function (e) {
                const message = this.dataset.confirm || 'آیا مطمئن هستید؟';
                if (!confirm(message)) {
                    e.preventDefault();
                    e.stopPropagation();
                    return false;
                }
            });
        });
    }

    // ============ Toast Notifications ============
    window.showToast = function (message, type = 'info') {
        if (typeof Swal === 'undefined') {
            alert(message);
            return;
        }

        const icons = { success: 'success', error: 'error', warning: 'warning', info: 'info' };
        const titles = { success: 'موفق', error: 'خطا', warning: 'هشدار', info: 'اطلاع' };

        Swal.fire({
            icon: icons[type] || 'info',
            title: titles[type] || 'اطلاع',
            text: message,
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
    };

    // ============ Copy to Clipboard ============
    window.copyToClipboard = function (text) {
        if (navigator.clipboard) {
            navigator.clipboard.writeText(text).then(() => {
                showToast('کپی شد!', 'success');
            });
        } else {
            const textarea = document.createElement('textarea');
            textarea.value = text;
            document.body.appendChild(textarea);
            textarea.select();
            document.execCommand('copy');
            document.body.removeChild(textarea);
            showToast('کپی شد!', 'success');
        }
    };

    // ============ Debounce Helper ============
    window.debounce = function (fn, delay = 300) {
        let timer;
        return function (...args) {
            clearTimeout(timer);
            timer = setTimeout(() => fn.apply(this, args), delay);
        };
    };

    // ============ Number Formatting (Persian) ============
    window.formatPersianNumber = function (num) {
        return new Intl.NumberFormat('fa-IR').format(num);
    };

    // ============ Init on DOM Ready ============
    function init() {
        initLazyImages();
        initConfirmDelete();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // ============ Re-init after HTMX swap ============
    document.body?.addEventListener('htmx:afterSwap', function () {
        initLazyImages();
        initConfirmDelete();
    });
})();