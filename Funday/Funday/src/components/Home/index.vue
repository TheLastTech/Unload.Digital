<template>
    <div class=" ">


        <h1 class="major">A Generic Page</h1>
        <span class="image fit"><img src="images/pic04.jpg" alt=""/></span>
        <p>Donec eget ex magna. Interdum et malesuada fames ac ante ipsum primis in faucibus. Pellentesque
            venenatis dolor imperdiet dolor mattis sagittis. Praesent rutrum sem diam, vitae egestas enim
            auctor sit amet. Pellentesque leo mauris, consectetur id ipsum sit amet, fergiat. Pellentesque
            in mi eu massa lacinia malesuada et a elit. Donec urna ex, lacinia in purus ac, pretium pulvinar
            mauris. Curabitur sapien risus, commodo eget turpis at, elementum convallis elit. Pellentesque
            enim turpis, hendrerit tristique.</p>
        <p>Interdum et malesuada fames ac ante ipsum primis in faucibus. Pellentesque venenatis dolor
            imperdiet dolor mattis sagittis. Praesent rutrum sem diam, vitae egestas enim auctor sit amet.
            Pellentesque leo mauris, consectetur id ipsum sit amet, fersapien risus, commodo eget turpis at,
            elementum convallis elit. Pellentesque enim turpis, hendrerit tristique lorem ipsum dolor.</p>




    </div>
</template>

<script lang="ts">
    import {Vue, Component, Prop} from "vue-property-decorator";

    import {store, signout} from "../../shared";
    import {Routes} from "@/shared/router";

    @Component({
        components: {},
    })
    export class Home extends Vue {
        @Prop({default: ""}) name: string;
        signout = signout;

        get userSession() {

            return store.userSession;
        }

        get SignOut() {
            return signout;
        }

        get Routes() {
            return Routes;
        }

        get store() {
            return store;
        }

    }

    export default Home;
</script>
