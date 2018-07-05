<template>
  <div>
    <div class="md-layout md-gutter">
      <div class="md-layout-item">
        <md-autocomplete v-model="currentSet" :md-options="setNames" @md-selected="getCards" md-input-placeholder="Type here...">
          <label>Set</label>
        </md-autocomplete>
      </div>
    </div>

    <md-table>
      <md-table-row>
        <md-table-head>Name</md-table-head>
        <md-table-head>Mana cost</md-table-head>
      </md-table-row>

      <md-table-row v-for="card in cards">
        <md-table-cell v-tooltip.top-center="{content: getCardTooltip(card.imageUrl)}">{{card.name}}</md-table-cell>
        <md-table-cell>
          <!--move to another component-->
          
          <span v-if="card.manaCost != null && countInstances(card.manaCost, '{W}') > 0">
            <img v-for="i in countInstances(card.manaCost, '{W}')" style="width: 32px; height: 32px;" src="../assets/img/white.png" />
          </span>

          <span v-if="card.manaCost != null && countInstances(card.manaCost, '{U}') > 0">
            <img v-for="i in countInstances(card.manaCost, '{U}')" style="width: 32px; height: 32px;" src="../assets/img/blue.png" />
          </span>

          <span v-if="card.manaCost != null && countInstances(card.manaCost, '{R}') > 0">
            <img v-for="i in countInstances(card.manaCost, '{R}')" style="width: 32px; height: 32px;" src="../assets/img/red.png" />
          </span>

        </md-table-cell>
        
      </md-table-row>
    </md-table>
  </div>
</template>

<script>
  const mtg = require('mtgsdk')

  export default {
    name: 'EditionsGrid', created() {
      this.getSets();
    },
    methods: {
      countInstances(string, word) {
        return string.split(word).length - 1;
      },
      getCardTooltip(image) {
        return "<img src='" + image +"'/>";
      },
      getSets() {
        this.sets = [];
        this.setNames = [];

        mtg.set.all()
          .on("data", set => {
            this.setNames.push(set.name)
            this.sets.push({ name: set.name, code: set.code })
          });
      },
      getCards() {
        this.cards = [];

        if (this.currentSet == null || this.currentSet == "")
          return;

        var set = this.sets.filter(x => x.name == this.currentSet)[0];

        mtg.card.all({ set: set.code })
          .on("data", card => {
            this.cards.push({
              name: card.name,
              manaCost: card.manaCost,
              imageUrl: card.imageUrl == null
                ? ""
                : card.imageUrl
            });
          });
      }
    },
    data() {
      return {
        cards: [],
        setNames: [],
        sets: [],
        currentSet: null
      }
    }
  }

</script>
