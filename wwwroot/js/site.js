//Scroll-Feature for a stickybar
const navE1 = document.querySelector('.navbar');

window.addEventListener('scroll', () => {
    if (window.scrollY >= 180) {
        navE1.classList.add('navbar-scrolled');
        navE1.classList.add('fixed-top');

    } else if (window.scrollY < 180) {
        //navE1.classList.remove('navbar-scrolled');
        navE1.classList.remove('fixed-top');

    }
});

