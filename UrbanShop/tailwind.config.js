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
      extend: {
          width: {
              "1/5": "20%",
              "4/6": "66.666667%",
              "23/100": "23%",
              "24/100": "24%",
          }
      },
  },
  variants: {
    extend: {},
  },
    plugins: [
        require('@tailwindcss/forms'),
    ],
}
