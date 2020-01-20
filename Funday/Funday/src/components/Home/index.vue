<template>
  <div class="row justify-content-between">
    <div class="col col-1">
      <i class="svg-home svg-9x"></i>
    </div>
    <div class="col col-4 mt-4">


    </div>
    <div class="col-md-auto"></div>
    <div class="col col-4">
      <div v-if="userSession" class="text-right">
        <p class="pt-3">Hi {{userSession.displayName}}!</p>
        <link-button @click="signout" sm primary>Sign Out</link-button>
      </div>

    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator';

import { store, signout } from '../../shared';

@Component({
  components: {

  },
})
export class Home extends Vue {
  @Prop({ default: '' }) name: string;
  signout = signout;

  get userSession() {
    return store.userSession;
  }

}
export default Home;
</script>
