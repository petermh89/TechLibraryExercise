<template>
    <div>
        <h1 style="text-align:center">Add New Book to library</h1>
        <br />
        <b-form-group>
            <b-form-input required v-model="book.title" placeholder="Title" :maxlength="100"></b-form-input>
        </b-form-group>
        <b-form-group>
            <b-form-input required v-model="book.ISBN" placeholder="ISBN" :maxlength="50"></b-form-input>
        </b-form-group>
        <b-form-group>
            <b-form-textarea required type="text" v-model="book.ShortDescr" rows="3" max-rows="6" :maxlength="1000" placeholder="Description"></b-form-textarea>
        </b-form-group>
        <h5 style="text-align:center">Add Image functionality coming soon!</h5>
        <b-form @submit="onSubmit" >
            <b-form-group>
                <b-form-file v-model="file"
                             :state="Boolean(file)"
                             placeholder="Drop Image or Browse"
                             drop-placeholder="Drop Image or Browse" accept="image/*"></b-form-file>
            </b-form-group>

            <b-button to="/" variant="primary">Back</b-button>
            <b-button type="submit" variant="success" class="float-right mr-2">Submit</b-button>
        </b-form>
    </div>
</template>

<script>
    import axios from 'axios';
    export default {
        name: 'AddBook',
        props: ["id"],
        data: () => ({
            book: [],
            error: null,
            file: null,
            baseUrl: 'https://localhost:5001/books',
        }),
        computed: {
            geturl() {
                return `${this.baseUrl}/new`
            }
        },
        mounted() {
            axios.get(this.geturl)
                .then(response => {
                    this.book = response.data;
                });
        },
        methods: {
            onSubmit(evt) {
                evt.preventDefault()
                axios.post(
                    `${this.baseUrl}/Add`,
                    {
                        Title: this.book.title,
                        ShortDescr: this.book.ShortDescr,
                        ThumbnailUrl: null,
                        ISBN: this.book.ISBN
                    })
                    .catch(e => {
                        this.updateError(e)
                    })
                    .finally(() => this.$router.push('/'))
            },
            updateError(err) {
                this.error = err
            },
        }
    }
</script>