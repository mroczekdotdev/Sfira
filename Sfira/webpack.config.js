const path = require("path");
const outputDir = "./wwwroot/"
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyPlugin = require("copy-webpack-plugin");

module.exports = {
  entry: "./webpack.index.js",
  output: {
    path: path.resolve(__dirname, outputDir),
    filename: "site.js",
  },
  module: {
    rules: [
      {
        test: /\.jsx?$/i,
        use: "babel-loader"
      },
      {
        test: /\.s[ac]ss$/i,
        use: [
          MiniCssExtractPlugin.loader,
          "css-loader",
          "sass-loader"
        ]
      }
    ]
  },
  resolve: {
    extensions: [".js", ".jsx", ".scss"]
  },
  plugins: [
    new CopyPlugin([
      { from: "node_modules/jquery/dist/jquery.min.js", to: "lib/jquery/" },
      { from: "node_modules/@fortawesome/fontawesome-free/js/all.min.js", to: "lib/font-awesome/" },
      { from: "node_modules/jquery-validation/dist/jquery.validate.min.js", to: "lib/jquery-validation/" },
      { from: "node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js", to: "lib/jquery-validation-unobtrusive/" }
    ]),
    new MiniCssExtractPlugin({
      filename: "site.css"
    })
  ]
};
