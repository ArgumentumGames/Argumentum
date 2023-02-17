const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
  mode: "production",
  entry: ['./src/scss/theme.scss', './src/ts/theme.ts'],
  output: {
    filename: 'theme.min.js',
    path: path.resolve(__dirname, 'dist'),
    assetModuleFilename: 'images/[hash][ext][query]'
  },
  devtool: 'source-map',
  watch: true,
  stats: {
    warnings: false,
    cachedModules: false,
    groupModulesByCacheStatus: false
  },
  cache: {
    type: 'filesystem',
    cacheDirectory: path.resolve(__dirname, '.temp_cache'),
    compression: 'gzip',
  },
  resolve: {
    extensions: ['.ts', '.tsx', '.js', '.scss']
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: 'theme.min.css',
    }),
    new webpack.ProgressPlugin(),
  ],
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: [
          {
            loader: 'ts-loader',
            options: {
              transpileOnly: true,
            },
          },
        ],
        exclude: /node_modules/,
      }, {
        test: /\.s[ac]ss$/i,
        use: [
          MiniCssExtractPlugin.loader,
          {
            loader: "css-loader",
            options: {
              sourceMap: true
            },
          }, {
            loader: 'postcss-loader',
            options: {
              sourceMap: true,
              postcssOptions: {
                plugins: function () {
                  return [
                    require('autoprefixer')
                  ];
                }
              }
            }
          }, {
            loader: "sass-loader",
            options: {
              sourceMap: true
            },
          }
        ],
      }
    ],
  },
};

new webpack.ProgressPlugin((percentage, message) => {
  console.log(`${(percentage * 100).toFixed()}% ${message}`);
})