//Scroll-Feature for a stickybar
const navE1 = document.querySelector('.navbar');

window.addEventListener('scroll', () => {
    if (window.scrollY >= 120) {
        navE1.classList.add('navbar-scrolled');

    } else if (window.scrollY < 120) {
        navE1.classList.remove('navbar-scrolled');
    }
});
