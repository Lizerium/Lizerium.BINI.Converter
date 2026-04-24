export default [
  {
    input: 'src/index.js',
    output: {
      file: 'dist/index.js',
      format: 'esm',
      sourcemap: true
    }
  },
  {
    input: 'src/index.js',
    output: {
      file: 'dist/index.cjs',
      format: 'cjs',
      exports: 'named',
      sourcemap: true
    }
  },
  {
    input: 'src/index.js',
    output: {
      file: 'dist/index.umd.js',
      format: 'umd',
      name: 'LizeriumBini',
      exports: 'named',
      sourcemap: true
    }
  }
];
