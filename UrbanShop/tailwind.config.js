module.exports = {
  purge: {
    enabled: true,
    content: [
      './Views/**/*.cshtml',
      './Pages/**/*.cshtml',
    ]
  },
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {},
  },
  variants: {
    extend: {},
  },
  plugins: [],
}
