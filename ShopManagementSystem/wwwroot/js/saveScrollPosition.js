 window.addEventListener('beforeunload', function () {
        sessionStorage.setItem('scrollPosition', window.scrollY);
 });

document.addEventListener('DOMContentLoaded', function () {
        var scrollPosition = sessionStorage.getItem('scrollPosition');
    if (scrollPosition) {
        window.scrollTo(0, parseInt(scrollPosition));
    sessionStorage.removeItem('scrollPosition');
    }
});

