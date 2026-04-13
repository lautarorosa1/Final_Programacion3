import { NumberService } from '@/services/NumberService.js';

export default {
  install(app) {
    // Esto permite usarlo en templates con {{ $formatoARS(valor) }}
    app.config.globalProperties.$formatoARS = NumberService.formatoARS;
  }
};