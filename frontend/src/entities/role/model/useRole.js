import { onMounted, ref } from "vue";
import { roleApi } from "../api/roleApi";

export const useRole = () => {
    const roles = ref([]);
    const isLoading = ref(false);

    const getRoles = async () => {
        isLoading.value = true;
        try {
            const response = await roleApi.getRoles();
            roles.value = response.data || response;
        } catch (e) {
            console.error(e);
        } finally {
            isLoading.value = false;
        }
    };

    onMounted(getRoles); 

    return { roles, isLoading };
};