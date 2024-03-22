import { Badge, MantineSize, MantineSpacing } from '@mantine/core';
import { currentYear } from 'app/util/dates';
import { serilogUiUrl } from 'app/util/prettyPrints';

const BrandBadge = ({ size, margin }: { margin?: MantineSpacing; size: MantineSize }) => {
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
