// Shared DeepDrive button system (.btn / .btn-primary / etc.) used by the
// AppButton component (Core/Shared/Components/Forms/AppButton.razor).
//
// Single source of truth: both the Website (src/Presentation/Website/tailwind.config.js)
// and the shared apps (src/Core/Shared/tailwind.config.js) wrap this callback with
// their own require('tailwindcss/plugin') so the compiled .btn* classes are identical
// in app.min.css (Website) and shared.css (Web / Web.Client / App).
//
// Exported as the addComponents callback (NOT pre-wrapped) so each config resolves
// 'tailwindcss/plugin' from its own node_modules.

module.exports = function ({ addComponents, theme }) {
  const transitionColors = {
    transitionProperty: 'color, background-color, border-color, text-decoration-color, fill, stroke',
    transitionTimingFunction: 'cubic-bezier(0.4, 0, 0.2, 1)',
    transitionDuration: '150ms',
  };

  addComponents({
    // Base layout/typography shared by every solid button.
    '.btn': {
      display: 'inline-flex',
      alignItems: 'center',
      justifyContent: 'center',
      gap: theme('spacing.2'),
      fontWeight: theme('fontWeight.bold'),
      fontFamily: '"Poppins", system-ui, sans-serif',
      textTransform: 'uppercase',
      letterSpacing: theme('letterSpacing.wider'),
      ...transitionColors,
      transitionProperty: 'color, background-color, border-color, box-shadow, transform',
    },
    // Colour variants — flat solid emerald, matching the app's bg-emerald-500 buttons
    // (no glow shadow: the emerald-tinted halo made it read as a brighter/different green).
    '.btn-primary': {
      backgroundColor: theme('colors.emerald.500'),
      color: theme('colors.white'),
      '&:hover': { backgroundColor: theme('colors.emerald.600') },
    },
    '.btn-dark': {
      backgroundColor: theme('colors.neutral.900'),
      color: theme('colors.white'),
      '&:hover': { backgroundColor: theme('colors.neutral.800') },
    },
    '.btn-ghost': {
      backgroundColor: 'rgba(255, 255, 255, 0.1)',
      color: theme('colors.white'),
      borderWidth: '1px',
      borderColor: 'rgba(255, 255, 255, 0.2)',
      '&:hover': { backgroundColor: 'rgba(255, 255, 255, 0.2)' },
    },
    // Outline (transparent) — white border/text for dark sections
    '.btn-outline': {
      borderWidth: '2px',
      borderColor: 'rgba(255, 255, 255, 0.3)',
      color: theme('colors.white'),
      '&:hover': { backgroundColor: 'rgba(255, 255, 255, 0.1)' },
    },
    // Outline (transparent) — neutral border/dark text for light sections
    '.btn-outline-dark': {
      borderWidth: '2px',
      borderColor: theme('colors.neutral.200'),
      color: theme('colors.neutral.900'),
      '&:hover': { backgroundColor: theme('colors.neutral.50') },
    },
    // Size variants (padding); lg also widens the icon gap.
    '.btn-sm': { padding: '0.625rem 1.25rem', fontSize: '0.875rem', lineHeight: '1.25rem' },
    '.btn-md': { padding: '0.75rem 2rem' },
    '.btn-lg': { padding: '1rem 2rem', gap: theme('spacing.3') },
    // Inline emerald arrow link ("View all" style)
    '.btn-link': {
      display: 'inline-flex',
      alignItems: 'center',
      gap: theme('spacing.2'),
      fontWeight: theme('fontWeight.bold'),
      fontFamily: '"Poppins", system-ui, sans-serif',
      textTransform: 'uppercase',
      letterSpacing: theme('letterSpacing.wider'),
      color: theme('colors.emerald.600'),
      '&:hover': { color: theme('colors.emerald.700') },
      ...transitionColors,
    },
  });
};
