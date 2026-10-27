// DeepDrive UI Kit — Tailwind theme (design tokens, fonts, dark "art-direction" palette).
// Single source of truth for both the library components and the demo app.
const path = require("path");
const p = (...s) => path.resolve(__dirname, ...s).replace(/\\/g, "/");
const plugin = require('tailwindcss/plugin');

// DeepDrive button system (.btn / .btn-primary / ...), consumed by the AppButton component.
const ddButtons = plugin(require('./tailwind.buttons.js'));

module.exports = {
    // Only apply hover: utilities on devices that actually support hover, so tap
    // states don't "stick" (look darker) on touch devices after pressing a button.
    future: {
        hoverOnlyWhenSupported: true,
    },
    safelist: [
        'max-w-sm', 'max-w-md', 'max-w-lg', 'max-w-xl', 'max-w-2xl', 'max-w-3xl', 'max-w-4xl',
    ],
    content: [
        // The library components…
        p("./**/*.razor"),
        p("./**/*.cshtml"),
        p("./**/*.cs"),
        // …and the demo app that exercises them.
        p("../../samples/DeepDrive.UiKit.Demo/**/*.razor"),
        p("../../samples/DeepDrive.UiKit.Demo/**/*.cshtml"),
        p("../../samples/DeepDrive.UiKit.Demo/**/*.cs"),
        p("../../samples/DeepDrive.UiKit.Demo/**/*.html"),
        "!" + p("**/bin/**"),
        "!" + p("**/obj/**"),
        "!" + p("**/node_modules/**")
    ],
    theme: {
        extend: {
            fontFamily: {
                sans: ['"Inter"', 'system-ui', 'sans-serif'],
                heading: ['"Poppins"', 'system-ui', 'sans-serif'],
                body: ['"Inter"', 'system-ui', 'sans-serif'],
                display: ['"Poppins"', 'system-ui', 'sans-serif'],
                ui: ['"Poppins"', 'system-ui', 'sans-serif'],
                // --- Dark "art direction" theme ---
                // Sora = display / oversized numerals; mono = instrumented data readouts.
                sora: ['"Sora"', 'system-ui', 'sans-serif'],
                mono: ['ui-monospace', 'SFMono-Regular', 'Menlo', 'monospace']
            },
            letterSpacing: {
                wider: "0.1em"
            },
            fontSize: {
                xs: ".75rem"
            },
            colors: {
                // brand == emerald (so brand-* and emerald-* are interchangeable accents).
                brand: {
                    50: '#ecfdf5', 100: '#d1fae5', 200: '#a7f3d0', 300: '#6ee7b7',
                    400: '#34d399', 500: '#10b981', 600: '#059669', 700: '#047857',
                    800: '#065f46', 900: '#064e3b', 950: '#022c22',
                },
                // Warm paper + warm ink monochrome; emerald stays the single accent.
                bone: '#fcfbf8',     // near-white warm paper page base
                bone2: '#f6f3ed',    // slightly deeper paper (subtle)
                ink: '#14120e',      // warm near-black (text + dark sections)
                inksoft: '#3a352c',  // softer ink
                taupe: '#8a8276',    // muted warm grey (meta text / labels)
                // Dark theme: canvas + cinematic plates.
                paper: '#e8e4dc',    // warm paper canvas (page base for dark theme)
                paper2: '#f2ede4',   // warm card paper on the canvas
                plate: '#0e0d0a',    // darkest cinematic plate (dock, hero base)
                plate2: '#221703',   // warm dark plate
                mist: '#bdb6a8',     // muted warm grey on paper
                mist2: '#cfc9be',    // lighter warm grey / hairlines
            },
            animation: {
                'subtle-pulse': 'subtle-pulse 3s ease-in-out infinite',
                'spin-slow': 'spin-slow 20s linear infinite',
            },
            keyframes: {
                'subtle-pulse': {
                    '0%, 100%': { opacity: '1' },
                    '50%': { opacity: '0.7' },
                },
                'spin-slow': {
                    from: { transform: 'rotate(0deg)' },
                    to: { transform: 'rotate(360deg)' },
                },
            },
            boxShadow: {
                'card': '0 0 0 1px rgba(0, 0, 0, 0.04), 0 4px 40px -12px rgba(0, 0, 0, 0.1)',
                'card-hover': '0 0 0 1px rgba(0, 0, 0, 0.04), 0 8px 50px -12px rgba(0, 0, 0, 0.15)',
            },
            zIndex: {
                '60': '60', '70': '70', '80': '80', '90': '90', '100': '100',
                'modal': '9999', 'modal-backdrop': '9998', 'overlay': '99999',
            },
            transitionTimingFunction: {
                'bounce-out': 'cubic-bezier(0.16, 1, 0.3, 1)',
            },
        }
    },
    plugins: [require('@tailwindcss/forms'), ddButtons]
};
