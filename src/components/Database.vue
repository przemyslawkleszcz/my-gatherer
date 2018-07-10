<template>
  <div>
    <h3>Database</h3>
  </div>
</template>

<script>
  export default {
    name: 'Database',
    methods: {
      findCard() {
        mtg.card.find({ name: "Life" })
          .then(result => {
            console.log(result.card.name);
          });
      }
    }
  }
</script>
