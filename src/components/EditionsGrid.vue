<template>
  <div>
    <div class="md-layout md-gutter">
      <div class="md-layout-item">
        <autocomplete source="https://api.magicthegathering.io/v1/sets?name="
                      ref="autocomplete"
                      placeholder="Search sets"
                      results-property="sets"
                      results-display="name"
                      resultsValue="code"
                      @selected="selected"
                      style="width: 300px; height: 80px;">
        </autocomplete>
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
          <mana :manaCost="card.manaCost"></mana>
        </md-table-cell>

      </md-table-row>
    </md-table>
  </div>
</template>

<script>
  const mtg = require('mtgsdk')
  import Mana from '@/components/Mana'
  import Autocomplete from 'vuejs-auto-complete'

  export default {
    name: 'EditionsGrid',
    components: {
      Mana,
      Autocomplete
    },
    methods: {
      getCardTooltip(image) {
        return "<img src='" + image + "'/>";
      },
      selected(result) {
        this.cards = [];

        mtg.card.all({ set: result.value })
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
        cards: []
      }
    }
  }

</script>
