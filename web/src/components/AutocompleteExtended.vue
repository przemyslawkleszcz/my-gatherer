<script>
  import Autocomplete from 'vuejs-auto-complete';
  import debounce from 'lodash/debounce';

  export default {
    name: 'AutocompleteExtended',
    extends: Autocomplete,
    components: {
      Autocomplete
    },
    methods: {
      resourceSearch: debounce(function (url) {
        if (!this.display) {
          this.results = []
          return
        }

        var self = this;
        if (url.then != null)
          url.then(function (x) {
            self.loading = !0;
            self.setEventListener();
            self.request(x);
          });
        else {
          self.loading = true;
          self.setEventListener();
          self.request(url);
        }
      }, 200)
    }
  }
</script>
