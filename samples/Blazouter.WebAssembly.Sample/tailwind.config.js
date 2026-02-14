/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./**/*.{cs,razor,html,cshtml}",
        "./../Blazouter.LazyModule.Sample/**/*.{cs,razor,html,cshtml}",
    ],
    safelist: [],
    darkMode: 'class',
    theme: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('@tailwindcss/typography'),
    ],
}