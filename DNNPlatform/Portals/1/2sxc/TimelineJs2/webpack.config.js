const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const FriendlyErrorsWebpackPlugin = require("friendly-errors-webpack-plugin");
const TerserPlugin = require("terser-webpack-plugin");
const WebpackBar = require("webpackbar");

module.exports = {
  entry: {
    styles: `./src/styles/styles.css`,
    scripts: "./src/ts/index.ts",
  },
  output: {
    path: path.resolve(__dirname, `dist`),
    filename: "[name].min.js",
  },
  mode: "production",
  devtool: "source-map",
  watch: true,
  stats: {
    all: false,
    assets: true,
  },
  resolve: {
    extensions: ['.ts', ".js", ".less", ".css"]
  },
  optimization: {
    minimize: true,
    minimizer: [
      new TerserPlugin({
        terserOptions: {
          output: {
            comments: false,
          },
        },
        extractComments: false,
      }),
      new OptimizeCSSAssetsPlugin({
        cssProcessorOptions: {
          map: {
            inline: false,
            annotation: true,
          },
        },
      }),
    ],
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: "[name].min.css",
    }),
    new WebpackBar(),
    new FriendlyErrorsWebpackPlugin(),
  ],
  module: {
    rules: [
      {
        test: /\.css$/,
        exclude: /node_modules/,
        use: [
          MiniCssExtractPlugin.loader,
          {
            loader: "css-loader",
            options: {
              sourceMap: true,
            },
          },
          {
            loader: "postcss-loader",
            options: {
              sourceMap: true,
              postcssOptions: {
                plugins: [require("autoprefixer")],
              },
            },
          },
        ],
      },
      {
        test: /\.less$/,
        use: [
          {
            loader: 'css-loader',
            options: {
                sourceMap: true,
            }
          },
          {
            loader: 'less-loader',
            options: {
                sourceMap: true,
            }
          },
        ],
      },
      {
        test: /\.ts$/,
        exclude: /node_modules/,
        use: {
          loader: 'ts-loader'
        }
      },
      {
        test: /\.(png|jpe?g|gif|woff(2)?|ttf|eot|svg)$/,
        type: 'asset/resource',
      },
    ],
  },
};
