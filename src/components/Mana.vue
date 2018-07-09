<template>
  <div>
    <span v-if="manaCost != null">
      <img v-for="i in countManaNumberTags(manaCost)" class="manaTag" :src="getSrc(i)" />
      <img v-for="i in countManaMainTags(manaCost, '{W}')" class="manaTag" src="../assets/img/white.svg" />
      <img v-for="i in countManaMainTags(manaCost, '{U}')" class="manaTag" src="../assets/img/blue.svg" />
      <img v-for="i in countManaMainTags(manaCost, '{R}')" class="manaTag" src="../assets/img/red.svg" />
      <img v-for="i in countManaMainTags(manaCost, '{B}')" class="manaTag" src="../assets/img/black.svg" />
      <img v-for="i in countManaMainTags(manaCost, '{G}')" class="manaTag" src="../assets/img/green.svg" />
    </span>
  </div>
</template>

<style scoped>
  .manaTag {
    width: 24px;
    height: 24px;
  }
</style>

<script>
  export default {
    name: 'Mana',
    props: ["manaCost"],
    methods: {
      getSrc(name) {
        try {
          var images = require.context('../assets/img/', false, /\.svg/);
          return images('./' + name + ".svg")
        } catch (err) { name + ".svg not found" }
      },
      countManaMainTags(string, word) {
        return string.split(word).length - 1;
      },
      countManaNumberTags(text) {
        var re = /{([0-9][0-9]*)}/g;
        var matches;
        var numbers = [];

        while (matches = re.exec(text))
          numbers.push(matches[1]);

        return numbers;
      }
    }
  }
</script>
