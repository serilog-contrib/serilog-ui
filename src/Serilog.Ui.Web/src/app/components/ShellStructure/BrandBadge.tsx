import { Badge, MantineSize, MantineSpacing } from '@mantine/core';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { currentYear } from 'app/util/dates';
import { serilogUiUrl } from 'app/util/prettyPrints';

const BrandBadge = ({ size, margin }: { margin?: MantineSpacing; size: MantineSize }) => {
  const { hideBrand } = useSerilogUiProps();

  if (hideBrand) return null;

  return (
    <Badge
      component="a"
      href={serilogUiUrl}
      style={{ cursor: 'pointer' }}
      target="_blank"
      size={size}
      color="cyan"
      m={margin}
      rightSection={currentYear}
    >
      Serilog UI
    </Badge>
  );
};

export default BrandBadge;
