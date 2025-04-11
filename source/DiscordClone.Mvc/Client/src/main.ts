import { createApp } from 'vue'

// mount Vue component dynamically (e.g. if using inside Razor)
document.querySelectorAll('[data-vue-component]').forEach((el) => {
    const componentName = el.getAttribute('data-vue-component');
    const props = JSON.parse(el.getAttribute('data-props') || '{}');

    import(`./pages/${componentName}.vue`).then((module) => {
        createApp(module.default, props).mount(el);
    });
});