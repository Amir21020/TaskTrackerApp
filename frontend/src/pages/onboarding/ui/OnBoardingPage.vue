<template>
  <AuthCard 
      title="Ваша должность?" 
      subtitle="Выберите вариант, чтобы настроить панель управления"
    >
      <OnboardingForm />
  </AuthCard>
</template>

<script setup>
import { AuthCard } from '@/widgets/auth-card';
import { OnboardingForm } from '@/features/onboarding'; 
</script>