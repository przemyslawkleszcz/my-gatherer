<template>
  <div>
    <h3>{{ msg }}</h3>
    <editions-grid></editions-grid>
  </div>
</template>
<script>
  import EditionsGrid from '@/components/EditionsGrid'

  export default {
    name: 'Editions',
    components: {
      EditionsGrid
    },
    data() {
      return {
        msg: "Editions"
      }
    }
  }
</script>

<style scoped>

  h1, h2 {
    font-weight: normal;
  }

  ul {
    list-style-type: none;
    padding: 0;
  }

  li {
    display: inline-block;
    margin: 0 10px;
  }

  a {
    color: #42b983;
  }
</style>
